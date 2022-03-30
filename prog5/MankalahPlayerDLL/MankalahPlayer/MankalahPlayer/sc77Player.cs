using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Mankalah
{
    /*****************************************************************/
    /*
    /* My own Personal Mankalah player. 
    /*
    /*****************************************************************/
    public class sc77Player : Player
    {
        int turn_count = 0;
        Position us;

        // sc77Player inherited from the given player class as a base
        public sc77(Position pos, int timeLimit) : base(pos, "sc77", timeLimit) { us = pos; }

        // Set a winning message
        public override string gloat()
        {
            return "sc77 won!!";
        }

        // Choose move in a given time
        public override int chooseMove(Board b)
        {
            turn_count++;
            //first and second moves
            if (turn_count == 1)
            {
                if (us == Position.Top)
                    return 9;
                else if (us == Position.Bottom)
                    return 2;
            }

            else if (turn_count == 2)
            {
                if (us == Position.Top)
                    return 12;
                else if (us == Position.Bottom)
                    return 5;
            }

            // create a stopwatch feature and move in according to the rule
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int depth_count = 5;
            Result move = new Result(0, 0);
            while (watch.ElapsedMilliseconds < getTimePerMove())
            {
                move = minimax(b, depth_count++, Int32.MinValue, Int32.MaxValue);
            }
            return move.BestMove();
        }

        /* Minimax search top player is max and bottom is min.
         * It will return a move from recursive calls. 
         */
        private Result minimax(Board board, int depth, int alpha, int beta)
        {
            if (board.gameOver() || depth == 0)
                return new Result(0, evaluate(board));

            int bestVal = int.MinValue;
            int bestMove = 0;
            if (board.whoseMove() == Position.Top)
            {
                for (int move = 7; move < 13 && alpha < beta; move++)
                {
                    if (board.legalMove(move))
                    {
                        Board b1 = new Board(board);
                        b1.makeMove(move, false);
                        Result val = minimax(b1, depth - 1, alpha, beta);
                        if (val.BestScore() > bestVal)
                        {
                            bestVal = val.BestScore();
                            bestMove = move;
                        }
                        if (bestVal > alpha)
                            alpha = bestVal;
                    }
                }
            }

            else
            {
                bestVal = Int32.MaxValue;
                for (int move = 0; move < 6 && alpha < beta; move++)
                {
                    if (board.legalMove(move))
                    {
                        Board b1 = new Board(board);
                        b1.makeMove(move, false);
                        Result val = minimax(b1, depth - 1, alpha, beta);
                        if (val.BestScore() < bestVal)
                        {
                            bestVal = val.BestScore();
                            bestMove = move;
                        }
                        if (bestVal < beta)
                            beta = bestVal;
                    }
                }
            }
            return new Result(bestMove, bestVal);
        }

        // evaluates the board from minimax search for total stones, possible replays and captures
        public override int evaluate(Board b)
        {
            int score = b.stonesAt(13) - b.stonesAt(6);
            int totalStones = 0;    //initialize the total stones on the board
            int playAgain = 0;      //initialize the total go-agains from ending in own bin
            int captures = 0;       //initialize the total number of possible captures
            int targetPit = 0;      //initialize the last pit into which stones from current pit will go


            if (b.whoseMove() == Position.Top)
            {
                for (int i = 7; i < 13; i++)
                {
                    totalStones += b.stonesAt(i);
                    if (b.stonesAt(i) == (13 - i))
                        playAgain++;

                    targetPit = i + b.stonesAt(i);

                    if (targetPit < 13 && (b.stonesAt(targetPit) == 0 && b.stonesAt(12 - i) > 0))
                        captures++;
                }

            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    totalStones -= b.stonesAt(i);
                    if (b.stonesAt(i) == (13 - i))
                        playAgain--;

                    targetPit = i + b.stonesAt(i);

                    if (targetPit < 13 && (b.stonesAt(targetPit) == 0 && b.stonesAt(12 - i) > 0))
                        captures--;
                }
            }
            return score + playAgain + captures + totalStones;
        }


        // gets an image for myself
        public override String getImage() { return "77.jpg"; }

    }

    /* Result class
     * Minimax search returns a result object containing the best move and predicted score
     */
    class Result
    {
        private int bestMove;
        private int bestScore;

        // take in move and score value as param and store it into a new variable to return and use it in the main game call.
        public Result(int move, int score)
        {
            bestMove = move;
            bestScore = score;
        }

        public int BestMove() { return bestMove; }
        public int BestScore() { return bestScore; }

    }
}