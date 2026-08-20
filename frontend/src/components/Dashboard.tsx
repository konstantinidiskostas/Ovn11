import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import {
    BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend,
    PieChart, Pie, Cell, ResponsiveContainer
} from "recharts";
import { getTopMoviesPerGenre, getAverageRating, getActiveActors } from "../services/movieService";
import type { TopGenreReport, AverageRatingReport, ActiveActorReport } from "../types/movie";

const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#8884d8', '#82ca9d', '#ffc658'];

export const Dashboard = () => {
    const [genreReport, setGenreReport] = useState<TopGenreReport[]>([]);
    const [ratingReport, setRatingReport] = useState<AverageRatingReport[]>([]);
    const [actorReport, setActorReport] = useState<ActiveActorReport[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchAll = async () => {
            try {
                const [genres, ratings, actors] = await Promise.all([
                    getTopMoviesPerGenre(),
                    getAverageRating(),
                    getActiveActors(),
                ]);
                setGenreReport(genres);
                setRatingReport(ratings);
                setActorReport(actors);
            } catch (err: any) {
                setError(err.message || 'Failed to load dashboard data');
            } finally {
                setLoading(false);
            }
        };
        fetchAll();
    }, []);

    if (loading) return <p>Loading dashboard...</p>;
    if (error) return <p>Error: {error}</p>;

    return (
        <div style={{ padding: '20px' }}>
            <Link to="/">← Back to movies</Link>
            <h1>Admin Dashboard</h1>

            <h2>Movies per Genre</h2>
            <ResponsiveContainer width="100%" height={300}>
                <BarChart data={genreReport}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="genre" />
                    <YAxis />
                    <Tooltip />
                    <Legend />
                    <Bar dataKey="movieCount" name="Movies" fill="#0088FE" />
                </BarChart>
            </ResponsiveContainer>

            <h2>Average Rating per Movie</h2>
            <ResponsiveContainer width="100%" height={300}>
                <BarChart data={ratingReport}>
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="title" />
                    <YAxis domain={[0, 5]} />
                    <Tooltip />
                    <Legend />
                    <Bar dataKey="averageRating" name="Avg Rating" fill="#00C49F" />
                </BarChart>
            </ResponsiveContainer>

            <h2>Most Active Actors</h2>
            <ResponsiveContainer width="100%" height={300}>
                <PieChart>
                    <Pie
                        data={actorReport}
                        dataKey="movieCount"
                        nameKey="name"
                        cx="50%"
                        cy="50%"
                        outerRadius={100}
                        label={({ name, movieCount }) => `${name} (${movieCount})`}
                    >
                        {actorReport.map((_, index) => (
                            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                        ))}
                    </Pie>
                    <Tooltip />
                    <Legend />
                </PieChart>
            </ResponsiveContainer>
        </div>
    );
};
