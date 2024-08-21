import React from 'react'
import * as ReactDOM from 'react-dom/client';  // ใช้ import แบบ named import 
import App from './App'
import './index.css'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
