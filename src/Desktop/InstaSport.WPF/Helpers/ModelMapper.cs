using InstaSport.Data.Models;
using InstaSport.WPF.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace InstaSport.WPF.Helpers
{
    public static class ModelMapper
    {
        public static SportDto ToDto(this Sport sport)
        {
            return new SportDto
            {
                Id = sport.Id,
                Name = sport.Name,
                TranslatedName = TranslateName(sport.NameTranslations, GetLanguage()) ?? sport.Name
            };
        }

        public static LocationDto ToDto(this Location location)
        {
            return new LocationDto
            {
                Id = location.Id,
                Name = TranslateName(location.NameTranslations, GetLanguage()) ?? location.Name,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                CityId = location.CityId,
                CityName = location.City.Name
            };
        }

        public static GameDto ToDto(this Game game)
        {
            return new GameDto
            {
                Id = game.Id,
                LocationId = game.LocationId,
                Location = game.Location.ToDto(),
                LocationName = TranslateName(game.Location.NameTranslations, GetLanguage()) ?? game.Location.Name,
                SportId = game.SportId,
                Sport = game.Sport.ToDto(),
                SportName = TranslateName(game.Sport.NameTranslations, GetLanguage()) ?? game.Sport.Name,
                StartingDateTime = game.StartingDateTime,
                Status = game.Status,
                MinPlayers = game.MinPlayers,
                MaxPlayers = game.MaxPlayers,
                Players = game.Players?.Select(p => p.ToDto()).ToList()
            };
        }

        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AvatarUrl = user.AvatarUrl
            };
        }

        public static IEnumerable<GameDto> ToDto(this IEnumerable<Game> games)
        {
            return games.ToList().Select(g => g.ToDto());
        }

        public static IEnumerable<SportDto> ToDto(this IEnumerable<Sport> sports)
        {
            return sports.ToList().Select(s => s.ToDto());
        }

        public static IEnumerable<LocationDto> ToDto(this IEnumerable<Location> locations)
        {
            return locations.ToList().Select(l => l.ToDto());
        }

        public static string? TranslateName(string nameTranslations, string language)
        {
            if (nameTranslations == null)
            {
                return null;
            }

            var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(nameTranslations);
            if (dictionary != null && dictionary.TryGetValue(language, out string? translatedName))
            {
                return translatedName;
            }

            return null;
        }

        public static string GetLanguage()
        {
            return CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        }
    }
}
