const Header = () => {    
    return (
      <header className="bg-gray-900 text-white py-4 px-6 sticky top-0 z-10">
        <div className="flex flex-col md:flex-row justify-between items-center">
          <div className="flex items-center mb-4 md:mb-0">
            <h1 className="text-2xl font-bold text-red-600 mr-8">My Movies</h1>
            <nav className="hidden md:flex space-x-6">
              <a href="#" className="hover:text-red-500">Home</a>
              <a href="#" className="hover:text-red-500">Movies</a>
              <a href="#" className="hover:text-red-500">TV Shows</a>
            </nav>
          </div>
        </div>
      </header>
    );
  };
  
export default Header;