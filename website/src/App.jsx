import { useState, useEffect } from 'react'
import './App.css'
import Navbar from './components/Navbar'
import Sidebar from './components/Sidebar'
import AIPanel from './components/AIPanel'
import MainPage from './components/MainPage'
import Login from './components/Login'

function App() {
  const [isOpen, setIsOpen] = useState(1);
  const [loginState, setLoginState] = useState('loading');
  const [mailContent, setMailContent] = useState({
    sender: "",
    subject: "",
    date: "",
    body: "",
    id: ""
  });

  useEffect(() => {
    (async () => {
      const response = await fetch('http://localhost:81/api/auth/verify');
      setLoginState(response.status === 200);
    })();
  }, [])

  function toggle() {
    setIsOpen(!isOpen);
  }

  function selectMail(sender, subject, date, body, id) {
    setMailContent({sender, subject, date, body, id})

    // Upload Context
    fetch('http://localhost:81/ai/switch', {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify({document: JSON.stringify({sender, subject, date, body})})
    })
  }

  // Returns the blank page when loading
  if (loginState === 'loading') return <></>;

  return (
    <>
      {loginState ?
        <>
          <Navbar />
          <Sidebar toggle={toggle} select={selectMail} isOpen={isOpen} />
          <AIPanel mailContent={mailContent}/>
          <MainPage isOpen={isOpen} mailContent={mailContent} />
        </>
        :
        <Login />}
    </>
  )
}

export default App
