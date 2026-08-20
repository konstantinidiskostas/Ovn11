const API_URL = 'http://localhost:5106'

export interface Actor {
  id: number;
  name: string;
}

// 2. Ανάκτηση όλων των διαθέσιμων ηθοποιών
export const getAllActors = async (): Promise<Actor[]> => {
  const response = await fetch('${API_URL}/api/v1/Actor');
  if (!response.ok) throw new Error('Error loading actors');
  return response.json();
};


export const addActorToMovie = async (movieId: number, actorId: number, role: string) => {
  const response = await fetch(
    `${API_URL}/api/v1/Movie/${movieId}/actors/${actorId}`,
    {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ role }),
    }
  );

  if (!response.ok) throw new Error('Error adding an actor');
  return response.json();
};