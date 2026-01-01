
import React, { useState } from 'react'
import Login from './pages/Login.jsx'
import Register from './pages/Register.jsx'
import Reminders from './pages/Reminders.jsx'

export default function App(){
  const [token, setToken] = useState(localStorage.getItem('token') || '')
  const [view, setView] = useState(token ? 'reminders' : 'login')

  const onLoggedIn = (t) => { localStorage.setItem('token', t); setToken(t); setView('reminders'); }
  const logout = () => { localStorage.removeItem('token'); setToken(''); setView('login'); }

  return (
    <div style={{ maxWidth: 640, margin: '20px auto', fontFamily: 'system-ui' }}>
      <h1>Reminder App</h1>
      <nav style={{ marginBottom: 20 }}>
        {view !== 'reminders' && <button onClick={()=>setView('login')}>Login</button>}
        {view !== 'reminders' && <button onClick={()=>setView('register')} style={{ marginLeft: 8 }}>Register</button>}
        {view === 'reminders' && <button onClick={logout}>Logout</button>}
      </nav>
      {view === 'login' && <Login onLoggedIn={onLoggedIn} />}
      {view === 'register' && <Register onRegistered={()=>setView('login')} />}
      {view === 'reminders' && <Reminders token={token} />}
    </div>
  )
}
