'use client'
import { useState, useEffect } from 'react';
import axios from "axios";

// export default function fileListComponent({data: {id, fileName, dateAdd, state}}: any) {
  export default function fileListComponent(data: any) {
    const [files, setFiles] = useState({});

    useEffect(() => {

      axios
      .get('http://localhost:5000/api/files')
      .then((res) => {
        console.log(res.data);
        setFiles(res.data);
      })
  
  });
    return (
      // <div>
      //   {/* {id} : {fileName} : {dateAdd} : {state} */}
      // </div>
      <div>
    </div>
    );
  }

interface FileProps {
    data: {
      id: number;
      fileName: string;
      dateAdd: string;
      state: string;      
    };
  }