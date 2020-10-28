using System;
using NLog.Web;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MediaLibrary
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {

            logger.Info("Program started");

            string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
            MovieFile movieFile = new MovieFile(scrubbedFile);

            string choice;
            do {
                // display options
                Console.WriteLine("1) Add movie");
                Console.WriteLine("2) Display all movies");
                Console.WriteLine("3) Search for movie");
                Console.WriteLine("Enter to quit");
                // input selection
                choice = Console.ReadLine();
                if (choice == "1"){
                    // add new movie
                    // ask user to input movie title
                    Console.Write("Enter title: ");
                    string title = Console.ReadLine();
                    // input genres
                    List<string> genres = new List<string>();
                    string genre;
                    do {
                        // ask user to enter genre
                        Console.Write("Enter genre (done to quit): ");
                        // input genre
                        genre = Console.ReadLine();
                        if (genre != "done"){
                            genres.Add(genre);
                        }
                    } while (genre != "done");
                    if (genres.Count == 0){
                        genres.Add("(no genres listed)");
                    }
                    // input director
                    Console.Write("Enter director: ");
                    string director = Console.ReadLine();

                    // input runtime
                    Console.Write("Enter runtime (h:m:s): ");
                    string runtime = Console.ReadLine();
                    //runtime = runtime.Length == 0 ? "00:00:00" : runtime;
                    // add movie
                    Movie movie = new Movie();
                    movie.title = title;
                    movie.genres = genres;
                    movie.director = director.Length == 0 ? "unassigned" : director;
                    movie.runningTime = TimeSpan.Parse(runtime);
                    movieFile.AddMovie(movie);
                } else if (choice == "2"){
                    // display all movies
                    // loop thru Movie List
                    for (int i = 0; i < movieFile.Movies.Count; i++){
                        Console.WriteLine(movieFile.Movies[i].Display());
                    }
                } else if (choice == "3"){
                    // movie search
                    Console.Write("Enter part of the movie title: ");
                    string titlePart = Console.ReadLine();
                    var movies = movieFile.Movies.Where(m => m.title.Contains(titlePart));
                    Console.WriteLine($"There are {movies.Count()} movies that match your search.");
                    foreach(Movie m in movies){
                        Console.WriteLine(m.Display());
                    }
                }
            } while (choice == "1" || choice == "2" || choice == "3");
            
            //RunLinqExamples(movieFile);
            logger.Info("Program ended");
        }

        static void RunLinqExamples(MovieFile movieFile){
             Console.ForegroundColor = ConsoleColor.Green;

            // LINQ - Where filter operator & Contains quantifier operator
            var Movies = movieFile.Movies.Where(m => m.title.Contains("(1990)"));
            // LINQ - Count aggregation method
            Console.WriteLine($"There are {Movies.Count()} movies from 1990");

            // LINQ - Any quantifier operator & Contains quantifier operator
            var validate = movieFile.Movies.Any(m => m.title.Contains("(1921)"));
            Console.WriteLine($"Any movies from 1921? {validate}");

            // LINQ - Where filter operator & Contains quantifier operator & Count aggregation method
            int num = movieFile.Movies.Where(m => m.title.Contains("(1921)")).Count();
            Console.WriteLine($"There are {num} movies from 1921");

            // LINQ - Where filter operator & Contains quantifier operator
            var Movies1921 = movieFile.Movies.Where(m => m.title.Contains("(1921)"));
            foreach(Movie m in Movies1921)
            {
                Console.WriteLine($"  {m.title}");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
