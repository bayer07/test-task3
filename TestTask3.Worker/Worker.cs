using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestTask3.Common.Entities;
using TestTask3.Common.Enums;

namespace TestTask3.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly int _delay;
        private readonly int _chunckSize;
        private readonly IDbConnection _connection;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IDbConnection connection)
        {
            _logger = logger;
            _delay = configuration.GetSection("delayMs").Get<int>();
            _chunckSize = configuration.GetSection("chunckSize").Get<int>();
            _connection = connection;
        }

        private async Task GetPdfBytes(FileUploadEntity file, IBrowser browser)
        {
            using (var page = await browser.NewPageAsync())
            {
                string html = Encoding.Unicode.GetString(file.Content, 0, file.Content.Length);
                await page.SetContentAsync(html);
                var result = await page.GetContentAsync();
                byte[] pdf = await page.PdfDataAsync();
                file.PdfContent = pdf;
                file.State = FileState.Handled;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Stopwatch sw = Stopwatch.StartNew();
                try
                {
                    _logger.LogDebug("Worker running at: {time}", DateTimeOffset.Now);
                    List<FileUploadEntity> files;

                    // Todo: this solution of parallel work can be improved by adding queue. That gives more transperency of the process.
                    using (var transaction = _connection.BeginTransaction(IsolationLevel.Serializable))
                    {
                        files = (await _connection.QueryAsync<FileUploadEntity>(
                            "DECLARE @ChunkTable TABLE([id] int);" +
                            "INSERT INTO @ChunkTable SELECT TOP(@top)[id] FROM Files WHERE [state] = @state;" +
                            "UPDATE Files SET[state] = 1 WHERE[id] IN(SELECT[id] FROM @ChunkTable);" +
                            "SELECT * FROM Files WHERE [id] IN (SELECT [id] FROM @ChunkTable);",
                            param: new { top = _chunckSize, state = FileState.Added }, transaction: transaction)).AsList();


                        transaction.Commit();
                    }
                    if (files.Count > 0)
                    {
                        var tasks = new List<Task>();
                        _logger.LogInformation($"Got files with identities:{string.Join(",", files.Select(x => x.Id))}");
                        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                        {
                            Headless = true,
                            ExecutablePath = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe" // change in case when your OS isn't windows
                        });

                        foreach (var file in files)
                        {
                            file.State = FileState.InProgress;
                            var task = Task.Run(() => GetPdfBytes(file, browser));
                            tasks.Add(task);
                        }

                        Task.WaitAll(tasks.ToArray());

                        using (var transaction = _connection.BeginTransaction())
                        {
                            foreach (var file in files)
                            {
                                _connection.Execute("UPDATE Files SET pdf_content = @content, state = @state WHERE id = @id; ", new { content = file.PdfContent, state = file.State, id = file.Id }, transaction: transaction);
                                _logger.LogInformation($"Handled a file with id:{file.Id}");
                            }

                            transaction.Commit();
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message, e);
                }
                finally
                {
                    _logger.LogTrace($"Execution time is {sw.ElapsedMilliseconds} ms");
                    await Task.Delay(_delay, stoppingToken);
                    sw = Stopwatch.StartNew();
                }
            }
        }
    }
}
