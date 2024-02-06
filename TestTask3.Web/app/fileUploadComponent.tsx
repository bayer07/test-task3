"use client";

import axios from "axios";
import { ChangeEvent } from 'react';

export default function FileUploadSingle() {
    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
      if (e.target.files) {
        let formData = new FormData();
        formData.append('customFile', e.target.files[0]);
        axios.post(
            'http://localhost:5000/api/files',
            formData,
            {
                headers: {              
                    "Content-type": "multipart/form-data",
                },                    
            }
        )
        .then(res => {
            console.log(`Success` + res.data);
        })
        .catch(err => {
            console.log(err);
        });
      }
    };
  
    return (
      <div>
        <input type="file" onChange={handleFileChange} accept=".html"/>        
      </div>
    );
  }