using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoSoftTest.ClassContainer
{
    public class CoordinatePlane
    {

        /// <summary>
        /// Все спички на поле
        /// </summary>
        public List<Match> matches = new List<Match>();

        
        /// <summary>
        /// Развивался ли огонь в этом фрейме
        /// </summary>
        public bool FrameFireMove
        {
            get
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].FrameHaveFire) return true;
                }
                return false;
            }
        }

        public void Fire()
        {

            

            // Распространение огня по спичкам
            foreach (var match in matches)
            {

                match.Fire();

            }

            // Передача огня на соседние спички
            foreach (var match in matches)
            {
                // Если передняя часть (по x1 y1) в огне и подожглась она уже давно
                if (match.isX1OnFire && !match.FrameX1Rate)
                {
                    // То передаем огонь на соседнюю спичку
                    FireOn(match.x1.x, match.x1.y);
                }
                // Если задняя часть в огне и подожглась она давно
                if (match.isX2OnFire && !match.FrameX2Rate)
                {
                    FireOn(match.x2.x, match.x2.y);
                }

                // Если центр в огне и поджегся давно (для больших спичек)
                if (match.isBigMatch && match.isCenterOnFire && !match.FrameCenterRate)
                {
                    FireOn(match.centerSquare(), match);
                }
            }


        }

        /// <summary>
        /// Поджечь все спички на этой координате (целочисленная)
        /// </summary>
        /// <param name="coord"></param>
        public void FireOn(int x, int y)
        {
            foreach(var match in matches)
            {
                match.FireOn(x, y);
            }
        }

        /// <summary>
        /// Поджечь все спички на этой координате (с плавающей точкой)
        /// </summary>
        /// <param name="coord"></param>
        public void FireOn(CoordFloat coords, Match m)
        {
            foreach (var match in matches)
            {
                if (m == match) continue;
                match.FireOn(coords.x, coords.y);
            }
        }

        /// <summary>
        /// Все ли спички сгорели?
        /// </summary>
        public bool IsAllBurned
        {
            get
            {
                foreach(var m in matches)
                {
                    if (!m.burned) return false;
                }
                return true;
            }
        }

        public CoordinatePlane GetCopy()
        {
            List<Match> newMatches = new List<Match>();
            foreach(var match in matches)
            {
                newMatches.Add(match.GetClone());
            }
            CoordinatePlane newPlane = new CoordinatePlane();
            newPlane.matches = newMatches;
            return newPlane;
        }
    }

}
