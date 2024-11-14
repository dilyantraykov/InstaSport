using InstaSport.Data.Models;
using System.Collections.Generic;
using System;
using InstaSport.WPF.Models;

public class GameDto
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public LocationDto Location { get; set; }
    public string LocationName { get; set; }
    public SportDto Sport { get; set; }
    public int SportId { get; set; }
    public string SportName { get; set; }
    public DateTime StartingDateTime { get; set; }
    public GameStatus Status { get; set; }
    public int? MinPlayers { get; set; }
    public int? MaxPlayers { get; set; }
    public ICollection<UserDto> Players { get; set; }
}
