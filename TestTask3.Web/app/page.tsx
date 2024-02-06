"use client";

import FileListComponent from './fileListComponent';
import FileUploadSingle from './fileUploadComponent';
import { useState, useEffect } from 'react';

export default function Home() {

  const [data, setData] = useState([])
  // useEffect(() => {
  //   axios
  //     .get('http://localhost:5000/api/files')
  //     .then((res) => {
  //       setData(res.data)
  //     })
  // })
//   const obj = {id: 1, fileName: 'asd', dateAdd: 'asd', state: 'asd'};

   return (
     <main>
       <div> 
       <FileListComponent data={null} />
        <FileUploadSingle></FileUploadSingle>
       </div>
     </main>
   );
}

