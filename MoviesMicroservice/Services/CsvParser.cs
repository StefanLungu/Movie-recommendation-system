using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using MoviesMicroservice.Entities;
using MoviesMicroservice.Helpers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MoviesMicroservice.Services
{
    public class CsvParser : ICsvParser
    {
        private List<MovieGenre> Genres { set; get; }
        private List<Movie> Movies { set; get; }

        private readonly AppSettings _appSettings;

        public CsvParser(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            Genres = new List<MovieGenre>();
            Movies = new List<Movie>();
            GenerateMovies();
        }

        private Dictionary<int, int> PrepareLinksMap()
        {
            Dictionary<int, int> linksMap = new Dictionary<int, int>();
            var parser = new TextFieldParser(_appSettings.LinksCsvPath)
            {
                TextFieldType = FieldType.Delimited
            };
            parser.SetDelimiters(new string[] { "," });
            parser.ReadLine();
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                try
                {
                    linksMap.Add(int.Parse(fields[0]), int.Parse(fields[2]));
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Tuple");
                }
            }
            return linksMap;
        }


        private void GenerateMovies()
        {
            var parser = new TextFieldParser(_appSettings.MoviesCsvPath)
            {
                TextFieldType = FieldType.Delimited
            };
            parser.SetDelimiters(new string[] { "," });
            int genreId = 1;
            string[] row = parser.ReadFields();
            Regex rx = new Regex(@"(\d{4})", RegexOptions.Compiled);
            MatchCollection matches;
            Dictionary<int, int> linksMap = PrepareLinksMap();
            while (!parser.EndOfData)
            {
                row = parser.ReadFields();
                int movieId = int.Parse(row[0]);
                matches = rx.Matches(row[1]);

                if (matches.Count == 0)
                {
                    continue;
                }

                if (!linksMap.ContainsKey(movieId))
                {
                    continue;
                }

                long tmdbId = linksMap[movieId];
                long releaseYear = long.Parse(matches[^1].ToString());
                string movieName = row[1].Remove(matches[^1].Index - 1);
                string[] genres = row[2].Split(new char[] { '|' });
                Movie movie = new Movie { Id = movieId, Title = movieName, ReleaseYear = releaseYear, TmdbId = tmdbId };

                foreach (string gen in genres)
                {
                    MovieGenre genre = Genres.Find(g => g.Name == gen);
                    if (genre == null)
                    {
                        genre = new MovieGenre { Name = gen, Id = genreId };
                        Genres.Add(genre);
                        genreId++;
                    }
                    genre.Movies.Add(movie);
                    movie.Genres.Add(genre);
                }
                Movies.Add(movie);
            }
        }

        public List<Movie> GetMovies()
        {
            return Movies;
        }
    }
}
