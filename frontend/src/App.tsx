import { useState } from 'react'
import { MovieList } from './components/MovieList'

import './App.css'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
    <MovieList />
      
    </>
  )
}

export default App
