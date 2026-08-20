import { useParams, Link } from "react-router-dom";
import { useEffect, useState } from "react";
import type { MovieDetails as MovieDetailsType, ReviewCreateDto } from "../types/movie";
import { createReview, getMovieDetails } from "../services/movieService";

export const MovieDetails = () => {
    const { id } = useParams<{id: string}>();

    const [movie, setMovie] = useState<MovieDetailsType | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    const [reviewForm, setReviewForm] = useState<ReviewCreateDto>({
        reviewerName: '',
        rating: 0,
        comment: '',
    });

    useEffect(() => {
        if (!id) return;

        const fetchData = async () => {
            try {
                setLoading(true);
                const data = await getMovieDetails(Number(id));
                setMovie(data);
                setError(null);
            } catch (err: any) {
                setError(err.message || 'Fail to load the movie');
            }
                finally {
                    setLoading(false);
                }
        };

        fetchData();
        }, [id]);
        if (loading) return <p>Loading data</p>;
        if (error) return <p>Error: {error}</p>;
        if (!movie) return <p>Movie </p>;

        const handleReviewChange = (
            e: React.ChangeEvent<HTMLInputElement>
        ) => {
            const { name, value, type } = e.target;
            setReviewForm((prev) => ({
                ...prev,
                [name]: type === 'number' ? Number(value) : value,
            }));
        };

        const handleReviewSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
            e.preventDefault();

            if (!id) return;

            try {
                const newReview = await createReview(Number(id), reviewForm);
                setMovie((prev) => {
                    if (!prev) return null;
                    return {
                        ...prev,
                    reviews: [...prev.reviews, newReview],
                };
            });
            setReviewForm({
                reviewerName: '',
                rating: 0,
                comment: '',
            });
            } catch (err: any) {
                alert(err.message || "Fail posting a review.")
            }
        };
    

    return (
        <div>
      <Link to="/">← back to home page</Link>

      <h2>{movie.title} ({movie.year})</h2>
      <p><strong>Genre:</strong> {movie.genre}</p>
      <p><strong>Duration:</strong> {movie.duration} minutes</p>
      <p><strong>Language:</strong> {movie.language}</p>
      <p><strong>Synopsis:</strong> {movie.synopsis}</p>
      <hr />

<form onSubmit={handleReviewSubmit}>
  <h3>Add review</h3>

  <div>
    <label>Name:</label>
    <input
      type="text"
      name="reviewerName"
      value={reviewForm.reviewerName}
      onChange={handleReviewChange}
      required
    />
  </div>

  <div>
    <label>Rating (1-5):</label>
    <input
      type="number"
      name="rating"
      min="1"
      max="5"
      value={reviewForm.rating}
      onChange={handleReviewChange}
      required
    />
  </div>

  <div>
    <label>Comment:</label>
    <input
      type="text"
      name="comment"
      value={reviewForm.comment}
      onChange={handleReviewChange}
      required
    />
  </div>

  <button type="submit">Submit review</button>
</form>
<h3>Reviews ({movie.reviews.length})</h3>

{movie.reviews.length === 0 ? (
  <p>There are no reviews</p>
) : (
  <ul>
    {movie.reviews.map((review) => (
      <li key={review.id} style={{ marginBottom: '10px' }}>
        <strong>{review.reviewerName}</strong> ({review.rating}/5)
        <br />
        <em>"{review.comment}"</em>
      </li>
    ))}
  </ul>
)}
    </div>
    );
};