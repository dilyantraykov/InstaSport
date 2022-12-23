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
        [Description("Waiting for Players")]
        WaitingForPlayers = 0,
        Playing = 1,
        Finished = 2
    }
}
