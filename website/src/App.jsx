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

  async function selectMail(folder, id) {
    // Get the message body
    const res = await fetch("http://localhost:81/api/email/email?" + new URLSearchParams({folder, emailID: id}).toString());
    const response = await res.json();

    setMailContent({sender: response.sender, subject: response.subject, date: response.date, body: response.body});
    console.log("Pressed");

    // Upload Context
    await fetch('http://localhost:81/ai/switch', {
      method: 'POST',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify({document: JSON.stringify({sender, subject, date, body: response})})
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
