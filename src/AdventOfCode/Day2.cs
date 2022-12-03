using Funk;

using NullableExtensions;

namespace AdventOfCode;

public static class Day2 {

    public static void Day2Part1() {
        var result =
            File.ReadAllLines(@"../../../../../../advent-of-code-2022/input/day2.txt")
                .Select(Day2.ComputeGame)
                .Aggregate(new TournamentScore(0, 0), TournamentScoreOps.Update);

        Console.WriteLine(result);
    }

    public static void Day2Part2() {
        var result =
            File.ReadAllLines(@"../../../../../../advent-of-code-2022/input/day2.txt")
                .Select(Day2.ComputeGame2)
                .Aggregate(new TournamentScore(0, 0), TournamentScoreOps.Update);

        Console.WriteLine(result);
    }

    public static (string PlayerCode, string OpponentCode) ParseCodes(string input) {
        var split = input.Split(' ');

        return (split[1], split[0]);
    }

    public static GameResult ComputeGame(string input) =>
        Fn.Apply(GameRoundOps.Init, Day2.ParseCodes(input))
            .OrElseThrow(new Exception("Bad Input"))
            .TotalScore();

    public static GameResult ComputeGame2(string input) =>
        Fn.Apply(GameInstructionOps.Init, Day2.ParseCodes(input))
            .OrElseThrow(new Exception("Bad Input"))
            .MakeRound()
            .TotalScore();

}

public enum HandShape {
    Rock,
    Paper,
    Scissors
}



public static class HandShapeOps {

    public static HandShape? FromOpponentCode(string code) =>
        code switch {
            "A" => HandShape.Rock,
            "B" => HandShape.Paper,
            "C" => HandShape.Scissors,
            _ => null
        };

    public static HandShape? FromPlayerCode(string code) =>
        code switch {
            "X" => HandShape.Rock,
            "Y" => HandShape.Paper,
            "Z" => HandShape.Scissors,
            _ => null
        };

    public static int Worth(this HandShape shape) =>
        shape switch {
            HandShape.Rock => 1,
            HandShape.Paper => 2,
            HandShape.Scissors => 3
        };
}

public record OutcomeResult(int Player, int Opponent);

public record GameResult(int Player, int Opponent);

public record GameRound(HandShape PlayerHand, HandShape OpponentHand);

public static class GameRoundOps {

    public static GameRound? Init(
        string playerCode,
        string opponentCode
    ) =>
        from player in HandShapeOps.FromPlayerCode(playerCode)
        from opponent in HandShapeOps.FromOpponentCode(opponentCode)
        select new GameRound(player, opponent);

    public static GameResult TotalScore(
        this GameRound round
    ) {
        var outcome = round.Outcome();
        return new(
            outcome.Player + round.PlayerHand.Worth(),
            outcome.Opponent + round.OpponentHand.Worth());
    }

    public static OutcomeResult Outcome(this GameRound round) =>
        round switch {
            GameRound { PlayerHand: HandShape.Rock, OpponentHand: HandShape.Paper } =>
                new OutcomeResult(0, 6),
            GameRound { PlayerHand: HandShape.Rock, OpponentHand: HandShape.Scissors } =>
                new OutcomeResult(6, 0),
            GameRound { PlayerHand: HandShape.Rock, OpponentHand: HandShape.Rock } =>
                new OutcomeResult(3, 3),

            GameRound { PlayerHand: HandShape.Paper, OpponentHand: HandShape.Paper } =>
                new OutcomeResult(3, 3),
            GameRound { PlayerHand: HandShape.Paper, OpponentHand: HandShape.Scissors } =>
                new OutcomeResult(0, 6),
            GameRound { PlayerHand: HandShape.Paper, OpponentHand: HandShape.Rock } =>
                new OutcomeResult(6, 0),

            GameRound { PlayerHand: HandShape.Scissors, OpponentHand: HandShape.Paper } =>
                new OutcomeResult(6, 0),
            GameRound { PlayerHand: HandShape.Scissors, OpponentHand: HandShape.Scissors } =>
                new OutcomeResult(3, 3),
            GameRound { PlayerHand: HandShape.Scissors, OpponentHand: HandShape.Rock } =>
                new OutcomeResult(0, 6),
            _ => throw new Exception("This really shouldn't be possible unless I'm a dummy")
        };
}


public enum PlayerCommand {
    Lose,
    Draw,
    Win
}


public static class PlayerCommandOps {

    public static PlayerCommand? FromPlayerCode(string code) =>
        code switch {
            "X" => PlayerCommand.Lose,
            "Y" => PlayerCommand.Draw,
            "Z" => PlayerCommand.Win,
            _ => null
        };
}


public record GameInstruction(PlayerCommand PlayerCommand, HandShape OpponentShape);

public static class GameInstructionOps {

    public static GameInstruction? Init(string playerCode, string opponentCode) =>
        from player in PlayerCommandOps.FromPlayerCode(playerCode)
        from opponent in HandShapeOps.FromOpponentCode(opponentCode)
        select new GameInstruction(player, opponent);

    public static GameRound MakeRound(
        this GameInstruction @this
    ) =>
        @this.OpponentShape switch {
            HandShape.Rock => @this.PlayerCommand switch {
                PlayerCommand.Lose => new(HandShape.Scissors, @this.OpponentShape),
                PlayerCommand.Draw => new(HandShape.Rock, @this.OpponentShape),
                PlayerCommand.Win => new(HandShape.Paper, @this.OpponentShape)
            },
            HandShape.Paper => @this.PlayerCommand switch {
                PlayerCommand.Lose => new(HandShape.Rock, @this.OpponentShape),
                PlayerCommand.Draw => new(HandShape.Paper, @this.OpponentShape),
                PlayerCommand.Win => new(HandShape.Scissors, @this.OpponentShape)
            },
            HandShape.Scissors => @this.PlayerCommand switch {
                PlayerCommand.Lose => new(HandShape.Paper, @this.OpponentShape),
                PlayerCommand.Draw => new(HandShape.Scissors, @this.OpponentShape),
                PlayerCommand.Win => new(HandShape.Rock, @this.OpponentShape)
            }
        };
}

public record TournamentScore(int Player, int Opponent);

public static class TournamentScoreOps {

    public static TournamentScore Update(
        this TournamentScore score,
        GameResult gameResult
    ) =>
        new(
            score.Player + gameResult.Player,
            score.Opponent + gameResult.Opponent);
}
