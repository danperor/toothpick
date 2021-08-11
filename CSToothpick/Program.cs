using System;

namespace CSToothpick
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] pieces = { 3, 5, 7 };
            int n = pieces.Length;
            PlayGame(pieces, TurnStatus.humanOne);
            Console.ReadKey();
        }
        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="pieces">数组</param>
        /// <param name="whoseTurn">先后顺序</param>
        static void PlayGame(int[] pieces, TurnStatus whoseTurn)
        {
            Console.WriteLine("Game Start!");

            Move Moves = new Move();

            while (GameOver(pieces) == false)
            {
                ShowPiles(pieces);

                MakeMove(pieces, ref Moves);

                if (whoseTurn == TurnStatus.humanOne)
                {
                    Console.WriteLine($"human1 take  { Moves.toothpick_removed} toothpicks from pieces at index:{Moves.piece_index} ");
                    whoseTurn = TurnStatus.humanSecond;
                }
                else
                {
                    Console.WriteLine($"human2 take  { Moves.toothpick_removed} toothpicks from pieces at index:{Moves.piece_index} ");
                    whoseTurn = TurnStatus.humanOne;
                }
            }

            ShowPiles(pieces);
            DeclareWinner(whoseTurn);
            return;
        }
        /// <summary>
        /// 取到所有的行都为空为止
        /// </summary>
        /// <param name="piles"></param>
        /// <returns></returns>
        static bool GameOver(int[] piles)
        {
            int i;
            for (i = 0; i < piles.Length; i++)
                if (piles[i] != 0)
                    return (false);
            return (true);
        }
        /// <summary>
        /// 显示当前的行状态
        /// </summary>
        /// <param name="piles"></param>
        static void ShowPiles(int[] piles)
        {
            int i;
            Console.WriteLine("Current Game Status -> ");
            for (i = 0; i < piles.Length; i++)
                Console.Write(" " + piles[i]);
            Console.WriteLine();
            return;
        }


        static void MakeMove(int[] pieces, ref Move moves)
        {
            int i, nim_sum = CalculateNimSum(pieces);

            //尝试得出最优解并使Nim-Sum 为0
            if (nim_sum != 0)
            {
                for (i = 0; i < pieces.Length; i++)
                {
                    if ((pieces[i] ^ nim_sum) < pieces[i])
                    {
                        moves.piece_index = i;
                        moves.toothpick_removed = pieces[i] - (pieces[i] ^ nim_sum);
                        pieces[i] = (pieces[i] ^ nim_sum);
                        break;
                    }
                }
            }
            else
            {
                int count;
                for (i = 0, count = 0; i < pieces.Length; i++)
                    if (pieces[i] > 0)
                        count++;

                moves.piece_index = new Random().Next(0, count);
                //在余下的不为0的行中取牙签
                int index = moves.piece_index;
                int pilesNum = pieces[index];
                while (pilesNum == 0)
                {
                    moves.piece_index = new Random().Next(0, count);
                    index = moves.piece_index;
                    pilesNum = pieces[index];
                }

                int removed = 1 + (new Random().Next() % (pieces[moves.piece_index]));

                moves.toothpick_removed = removed;
                pieces[moves.piece_index] = pieces[moves.piece_index] - moves.toothpick_removed;

                if (pieces[moves.piece_index] < 0)
                    pieces[moves.piece_index] = 0;
            }
        }
        static void DeclareWinner(TurnStatus whoseTurn)
        {
            if (whoseTurn == TurnStatus.humanOne)
                Console.WriteLine("human2 won");
            else
                Console.WriteLine("human1 won");
            return;
        }

        /// <summary>
        /// 计算nim和
        /// </summary>
        /// <param name="pieces"></param>
        /// <returns></returns>
        static int CalculateNimSum(int[] pieces)
        {
            int i, nimsum = pieces[0];
            for (i = 1; i < pieces.Length; i++)
                nimsum = nimsum ^ pieces[i];
            return (nimsum);
        }

    }
    enum TurnStatus
    {
        humanOne = 1,
        humanSecond
    }
    public struct Move
    {
        public int piece_index;
        public int toothpick_removed;
    }
}



