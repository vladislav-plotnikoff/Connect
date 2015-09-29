using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect
{
    class Core
    {
        /// <summary>
        /// Информация о клетке
        /// </summary>
        public class BaseElement
        {
            /// <summary>
            /// Наличие нижнего конектера
            /// </summary>
            public bool down { get; protected set; }

            /// <summary>
            /// Наличие левого конектера
            /// </summary>
            public bool left { get; protected set; }

            /// <summary>
            /// Блокировка
            /// </summary>
            public bool block { get; protected set; }

            /// <summary>
            /// Наличие сети
            /// </summary>
            public bool net { get; protected set; }

            /// <summary>
            /// Является ли узел ПК
            /// </summary>
            public bool pc { get; protected set; }

            /// <summary>
            /// Наличие правого конектера
            /// </summary>
            public bool right { get; protected set; }

            /// <summary>
            /// Является ли узел сервером
            /// </summary>
            public bool server { get; protected set; }

            /// <summary>
            /// Наличие верхнего конектера
            /// </summary>
            public bool up { get; protected set; }
        }

        /// <summary>
        /// Клетка в игре
        /// </summary>
        private class Element : BaseElement
        {
            /// <summary>
            /// Переменная для восстановления
            /// </summary>
            private bool rUp, rDown, rLeft, rRight;

            /// <summary>
            /// Переменная решения
            /// </summary>
            private bool oUp, oDown, oLeft, oRight;

            /// <summary>
            /// Определение нового элемента
            /// </summary>
            /// <param name="up"></param>
            /// <param name="down"></param>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <param name="server"></param>
            public Element(bool up, bool down, bool left, bool right, bool server)
            {
                this.up = up;
                this.down = down;
                this.left = left;
                this.right = right;

                //Сохрание начального положения
                oUp = up;
                oDown = down;
                oLeft = left;
                oRight = right;

                if (server)
                {
                    this.server = server;
                }
                else
                {
                    int count;
                    count = up ? 1 : 0;
                    count += down ? 1 : 0;
                    count += left ? 1 : 0;
                    count += right ? 1 : 0;
                    pc = count == 1;
                }
            }

            /// <summary>
            /// Предоставляет доступ к свойству наличия сети
            /// </summary>
            public new bool net { get { return base.net; } set { base.net = value; } }
            
            /// <summary>
            /// Восстановление верного решения
            /// </summary>
            public void CallHint()
            {
                up = oUp;
                down = oDown;
                left = oLeft;
                right = oRight;
                block = true;
            }

            /// <summary>
            /// Сохранение позиции для последующего восстановления
            /// </summary>
            public void SaveForRepeat()
            {
                rUp = up;
                rDown = down;
                rLeft = left;
                rRight = right;
            }

            /// <summary>
            /// Загрузка сохранённой позиции для повтора игры
            /// </summary>
            public void LoadForRepeat()
            {
                up = rUp;
                down = rDown;
                left = rLeft;
                right = rRight;
                block = false;
            }

            /* Проверяет правильность подключения
            public bool Check()
            {
                return oUp == up && oDown == down && oLeft == left && oRight == right;
            }
            */

            /// <summary>
            /// Заблокировать | Разблокировать элемент
            /// </summary>
            public void LockUnlock()
            {
                block = !block;
            }

            /// <summary>
            /// Поворот против часовой стрелке
            /// </summary>
            public void RotationLeft()
            {
                bool temp = up;
                up = left;
                left = down;
                down = right;
                right = temp;
            }

            /// <summary>
            /// Поворот по часовой стрелке
            /// </summary>
            public void RotationRight()
            {
                bool temp = up;
                up = right;
                right = down;
                down = left;
                left = temp;
            }
        }
        
        /// <summary>
        /// Типы сложности игры
        /// </summary>
        public enum Mode { Easy = 5, Normal = 7, Hard = 9, Expert = 9 }

        /// <summary>
        /// Тип хода
        /// </summary>
        public enum TypeTurn { left, right, block }

        /// <summary>
        /// Основное поле игры
        /// </summary>
        private Element[,] elements;

        /// <summary>
        /// Сложность игры
        /// </summary>
        public Mode mode { get; private set; }

        /// <summary>
        /// Количество неподключенных ПК
        /// </summary>
        private int countDisconnectedPC;

        /// <summary>
        /// Предоставляет доступ к элементам поля
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public BaseElement this[int x, int y]
        {
            get { return elements[x, y]; }
        }

        /// <summary>
        /// Создание новой игры
        /// </summary>
        public void NewGame()
        {
            elements = new Element[1, 1];
            elements[0, 0] = new Element(true, false, false, false, false);
            elements[0, 0].net = false;
        }

        /// <summary>
        /// Создание новой игры с другим уровнем сложности
        /// </summary>
        /// <param name="mode">Уровень сложности</param>
        public void NewGame(Mode mode)
        {

        }

        /// <summary>
        /// Повторить игру
        /// </summary>
        public void RepeatGame()
        {

        }

        /// <summary>
        /// Новый ход
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="typeTurn"></param>
        /// <returns>Возращает true усли ход победный</returns>
        public bool NewTurn(int x, int y, TypeTurn typeTurn)
        {
            return countDisconnectedPC == 0;
        }
    }
}