
import React, { useEffect, useState } from 'react'

export default function Reminders({ token }){
  const [list, setList] = useState([])
  const [text, setText] = useState('')
  const [when, setWhen] = useState('')

  const load = async () => {
    const res = await fetch('/api/reminders', {
      headers: { Authorization: `Bearer ${token}` }
    })
    const data = await res.json()
    setList(data)
  }

  useEffect(()=>{ load() },[])

  const add = async (e) => {
    e.preventDefault()
    const scheduledAtUtc = new Date(when).toISOString()
    await fetch('/api/reminders', {
      method: 'POST',
      headers: { 'Content-Type':'application/json', Authorization: `Bearer ${token}` },
      body: JSON.stringify({ text, scheduledAtUtc })
    })
    setText(''); setWhen('');
    load()
  }

  const del = async (id) => {
    await fetch(`/api/reminders/${id}`, { method: 'DELETE', headers: { Authorization: `Bearer ${token}` }})
    setList(list.filter(x=>x.id !== id))
  }

  return (
    <div>
      <h2>Your Reminders</h2>
      <form onSubmit={add}>
        <input placeholder='Reminder text' value={text} onChange={e=>setText(e.target.value)} required />
        <input type='datetime-local' value={when} onChange={e=>setWhen(e.target.value)} required />
        <button type='submit'>Add</button>
      </form>
      <ul>
        {list.map(r => (
          <li key={r.id}>
            {r.text} — {new Date(r.scheduledAt).toLocaleString()}
            <button style={{ marginLeft: 8 }} onClick={()=>del(r.id)}>Delete</button>
          </li>
        ))}
      </ul>
    </div>
  )
}
