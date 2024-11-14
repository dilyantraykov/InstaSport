using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaSport.Data.Models
{
    public enum GameStatus
    {
        [Description("{\"en\": \"Waiting for Players\", \"bg\": \"Изчаква играчи\"}")]
        WaitingForPlayers = 0,
        [Description("{\"en\": \"Playing\", \"bg\": \"Играе се\"}")]
        Playing = 1,
        [Description("{\"en\": \"Finished\", \"bg\": \"Приключила\"}")]
        Finished = 2
    }
}
