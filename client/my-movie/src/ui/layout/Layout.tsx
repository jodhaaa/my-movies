import MovieList from "../movies/MovieList";
import Header from "./Header";

const Layout: React.FC = () => {
    return (
        <div className="min-h-screen bg-gray-900">
            <Header />

            <main className="container mx-auto px-4 py-8">
                <MovieList></MovieList>
            </main>

            <footer className="bg-gray-900 text-gray-400 py-8 border-t border-gray-800">
                <div className="container mx-auto px-4">
                    <div className="flex flex-col md:flex-row justify-between">
                        <div className="mb-6 md:mb-0">
                            <h2 className="text-xl font-bold text-red-600 mb-4">My Movies</h2>
                            <p>© 2025 My Movies. All rights reserved.</p>
                        </div>
                        <div className="grid grid-cols-2 gap-8">
                            <div>
                                <h3 className="text-white font-medium mb-2">Company</h3>
                                <ul className="space-y-2">
                                    <li><a href="#" className="hover:text-white">About Us</a></li>
                                    <li><a href="#" className="hover:text-white">Help</a></li>
                                    <li><a href="#" className="hover:text-white">Contact</a></li>
                                </ul>
                            </div>
                            <div>
                                <h3 className="text-white font-medium mb-2">Connect</h3>
                                <ul className="space-y-2">
                                    <li><a href="#" className="hover:text-white">Twitter</a></li>
                                    <li><a href="#" className="hover:text-white">Instagram</a></li>
                                    <li><a href="#" className="hover:text-white">Facebook</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </footer>
        </div>
    );
}

export default Layout;