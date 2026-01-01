
import React, { useState } from 'react'

export default function Register({ onRegistered }){
  const [name, setName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [message, setMessage] = useState('')

  const submit = async (e) => {
    e.preventDefault()
    setMessage('')
    const res = await fetch('/api/auth/register', {
      method: 'POST', headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, email, password })
    })
    if(res.ok){
      setMessage('Registered! You can login now.')
      onRegistered()
    } else {
      const t = await res.text()
      setMessage(t)
    }
  }

  return (
    <form onSubmit={submit}>
      <label>Name<br/>
        <input value={name} onChange={e=>setName(e.target.value)} required />
      </label><br/>
      <label>Email<br/>
        <input value={email} onChange={e=>setEmail(e.target.value)} required />
      </label><br/>
      <label>Password<br/>
        <input type='password' value={password} onChange={e=>setPassword(e.target.value)} required />
      </label><br/>
      <button type='submit'>Register</button>
      {message && <p>{message}</p>}
    </form>
  )
}
