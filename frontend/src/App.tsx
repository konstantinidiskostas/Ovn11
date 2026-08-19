import { useState } from 'react'
import { MovieList } from './components/MovieList'

import './App.css'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { MovieDetails } from './components/MovieDetails'

function App() {
  const [count, setCount] = useState(0)

  return (
    <BrowserRouter>    
    <Routes>
      <Route path="/" element={<MovieList />} />
      <Route path="movies/:id" element={<MovieDetails />}/>
      </Routes>
      
    </BrowserRouter>
  )
}

export default App
