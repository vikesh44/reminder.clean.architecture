
import React, { useState } from 'react'

export default function Login({ onLoggedIn }){
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')

  const submit = async (e) => {
    e.preventDefault()
    setError('')
    const res = await fetch('/api/auth/login', {
      method: 'POST', headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    })
    if(res.ok){
      const { token } = await res.json()
      onLoggedIn(token)
    } else {
      const t = await res.json().catch(()=>({message:'Invalid'}))
      setError(t.message || 'Login failed')
    }
  }

  return (
    <form onSubmit={submit}>
      <label>Email<br/>
        <input value={email} onChange={e=>setEmail(e.target.value)} required />
      </label><br/>
      <label>Password<br/>
        <input type='password' value={password} onChange={e=>setPassword(e.target.value)} required />
      </label><br/>
      <button type='submit'>Login</button>
      {error && <p style={{color:'red'}}>{error}</p>}
    </form>
  )
}
