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

            /// <summary>
            /// Количество контактов
            /// </summary>
            public int connects
            {
                get
                {
                    return (up ? 1 : 0) + (down ? 1 : 0) + (left ? 1 : 0) + (right ? 1 : 0);
                }
            }
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
            /// Копирования элемента
            /// </summary>
            /// <param name="obj"></param>
            public void CopyElement(Element obj)
            {
                up = obj.up;
                down = obj.down;
                right = obj.right;
                left = obj.left;
            }
            
            /// <summary>
            /// Флаги направлений
            /// </summary>
            public Directions directions
            {
                get
                {
                    Directions d = (Directions)0x0;
                    if (up)
                        d = d | Directions.up;
                    if (down)
                        d = d | Directions.down;
                    if (left)
                        d = d | Directions.left;
                    if (right)
                        d = d | Directions.right;
                    return d;
                }
                set
                {
                    up = value.HasFlag(Directions.up);
                    down = value.HasFlag(Directions.down);
                    left = value.HasFlag(Directions.left);
                    right = value.HasFlag(Directions.right);
                }
            }

            /// <summary>
            /// Предоставляет доступ к свойству наличия сети
            /// </summary>
            public new bool net { get { return base.net; } set { base.net = value; } }

            /// <summary>
            /// Предоставляет доступ к свойству блокировки элемента
            /// </summary>
            public new bool block { get { return base.block; } set { base.block = value; } }

            /// <summary>
            /// Предоставляет доступ к свойству блокировки элемента
            /// </summary>
            public new bool pc { get { return base.pc; } set { base.pc = value; } }

            /// <summary>
            /// Предоставляет доступ к свойству блокировки элемента
            /// </summary>
            public new bool server { get { return base.server; } set { base.server = value; } }

            /// <summary>
            /// Предоставляет доступ к свойству блокировки элемента
            /// </summary>
            public new bool up { get { return base.up; } set { base.up = value; } }

            /// <summary>
            /// Предоставляет доступ к свойству блокировки элемента
            /// </summary>
            public new bool down { get { return base.down; } set { base.down = value; } }

            /// <summary>
            /// Предоставляет доступ к свойству блокировки элемента
            /// </summary>
            public new bool left { get { return base.left; } set { base.left = value; } }

            /// <summary>
            /// Предоставляет доступ к свойству блокировки элемента
            /// </summary>
            public new bool right { get { return base.right; } set { base.right = value; } }

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
        /// Точка
        /// </summary>
        private class Point
        {
            /// <summary>
            /// Координата X
            /// </summary>
            public int x;
            /// <summary>
            /// Координата Y
            /// </summary>
            public int y;

            /// <summary>
            /// Конструктор точки
            /// </summary>
            public Point(int x = 0, int y = 0)
            {
                this.x = x;
                this.y = y;
            }

            /// <summary>
            /// Конструктор копирования
            /// </summary>
            /// <param name="p"></param>
            public Point(Point p)
            {
                x = p.x;
                y = p.y;
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
        /// Направления
        /// </summary>
        [Flags]
        public enum Directions { up = 0x01, right = 0x02, down = 0x04, left = 0x8 }

        /// <summary>
        /// Класс хода
        /// </summary>
        private class Turn
        {
            public int x { get; private set; }
            public int y { get; private set; }
            public TypeTurn typeTurn { get; private set; }
            public Turn(int x, int y, TypeTurn typeTurn)
            {
                this.x = x;
                this.y = y;
                this.typeTurn = typeTurn;
            }
        }

        /// <summary>
        /// Основное поле игры
        /// </summary>
        private Element[,] elements;

        /// <summary>
        /// Шаблоны элементов
        /// </summary>
        private Element[] sampleElements;

        /// <summary>
        /// Сложность игры
        /// </summary>
        public Mode mode { get; private set; }

        /// <summary>
        /// Координата сервера
        /// </summary>
        private Point server;

        /// <summary>
        /// История ходов
        /// </summary>
        private List<Turn> history;

        /// <summary>
        /// Количество ходов выделенных для решения
        /// </summary>
        public int step { get; private set; }

        /// <summary>
        /// Количество ходов изначально выделенных для решения
        /// </summary>
        private int rStep;

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
            get
            {
                if (x < 0)
                {
                    x = (int)mode + x % (int)mode;
                }
                else if (x >= (int)mode)
                {
                    x = x % (int)mode;
                }
                if (y < 0)
                {
                    y = (int)mode + y % (int)mode;
                }
                else if (y >= (int)mode)
                {
                    y = y % (int)mode;
                }
                return elements[x, y];
            }
        }

        /// <summary>
        /// Предоставляет доступ к элементам поля
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Element this[Point p]
        {
            get
            {
                if (p.x < 0)
                {
                    p.x = (int)mode + p.x % (int)mode;
                }
                else if (p.x >= (int)mode)
                {
                    p.x = p.x % (int)mode;
                }
                if (p.y < 0)
                {
                    p.y = (int)mode + p.y % (int)mode;
                }
                else if (p.y >= (int)mode)
                {
                    p.y = p.y % (int)mode;
                }
                return elements[p.x, p.y];
            }
        } 

        /// <summary>
        /// Конструктор ядра
        /// </summary>
        public Core()
        {
            history = new List<Turn>();
            server = new Point();
            sampleElements = new Element[15];
            for (int i = 0; i < 15; i++)
            {
                sampleElements[i] = new Element();
                sampleElements[i].directions = (Directions)i;
            }
        }

        /// <summary>
        /// Создание новой игры
        /// </summary>
        public void NewGame()
        {
            elements = new Element[9, 9];
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
        /// Проверка наличия сети
        /// </summary>
        private void CheckConnected()
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
            history.Add(new Turn(x, y, typeTurn));
            switch (typeTurn)
            {
                case TypeTurn.block:
                    elements[x, y].LockUnlock();
                    break;
                case TypeTurn.left:
                    if (elements[x, y].block)
                    {
                        elements[x, y].RotationLeft();
                        step--;
                        CheckConnected();
                    }
                    break;
                case TypeTurn.right:
                    if (elements[x, y].block)
                    {
                        elements[x, y].RotationRight();
                        step--;
                        CheckConnected();
                    }
                    break;
            }
            return countDisconnectedPC == 0;
        }

        /// <summary>
        /// Сохраняет игру
        /// </summary>
        public void SaveGame()
        {

        }

        /// <summary>
        /// Загрузить игру
        /// </summary>
        public void LoadGame()
        {

        }

        /// <summary>
        /// Генерация лабиринта
        /// </summary>
        private void CreateField()
        {
            Random rand = new Random();
            server = new Point(rand.Next((int)mode), rand.Next((int)mode));
            elements[server.x, server.y].server = true;
            if (mode == Mode.Expert)
            {
                this[server].CopyElement(sampleElements[rand.Next(14) + 1]);
                List<Point> peaks = new List<Point>();
                Point tempPoint;
                if (this[server].up)
                {
                    tempPoint = new Point(server);
                    tempPoint.y--;
                    peaks.Add(tempPoint);
                    this[tempPoint].down = true;
                }
                if (this[server].down)
                {
                    tempPoint = new Point(server);
                    tempPoint.y++;
                    peaks.Add(tempPoint);
                    this[tempPoint].up = true;
                }
                if (this[server].left)
                {
                    tempPoint = new Point(server);
                    tempPoint.x--;
                    peaks.Add(tempPoint);
                    this[tempPoint].right = true;
                }
                if (this[server].right)
                {
                    tempPoint = new Point(server);
                    tempPoint.x++;
                    peaks.Add(tempPoint);
                    this[tempPoint].left = true;
                }
                Point peak;
                List<int> availableSamples = new List<int>();
                while (peaks.Count > 0)
                {
                    peak = peaks[rand.Next(peaks.Count)];
                    switch (this[peak].directions)
                    {
                        case Directions.up:
                            for (int i = 0; i < 15; i++)
                                if (sampleElements[i].up)
                                    availableSamples.Add(i);
                            break;
                        case Directions.down:
                            for (int i = 0; i < 15; i++)
                                if (sampleElements[i].down)
                                    availableSamples.Add(i);
                            break;
                        case Directions.left:
                            for (int i = 0; i < 15; i++)
                                if (sampleElements[i].left)
                                    availableSamples.Add(i);
                            break;
                        case Directions.right:
                            for (int i = 0; i < 15; i++)
                                if (sampleElements[i].right)
                                    availableSamples.Add(i);
                            break;
                    }
                    if (this[peak.x, peak.y - 1].connects > 0)
                    {
                        
                    }
                }
            }
            else
            {
                for (int i = 0; i < (int)mode; i++)
                    for (int j = 0; j < (int)mode; j++)
                    {
                        if (i != 0)
                            elements[i, j].left = true;
                        if (i != (int)mode - 1)
                            elements[i, j].right = true;
                        if (j != 0)
                            elements[i, j].up = true;
                        if (j != (int)mode - 1)
                            elements[i, j].down = true;
                    }
                int[,] multiplicity = new int[(int)mode, (int)mode];
                for (int i = 0; i < (int)mode; i++)
                    multiplicity[i, 0] = i;
                for (int j = 0; j < (int)mode - 1; j++)
                {
                    for (int i = 0; i < (int)mode - 1; i++)
                    {
                        if (multiplicity[i, j] == multiplicity[i + 1, j])
                        {
                            elements[i + 1, j].left = false;
                            elements[i, j].right = false;
                        }
                        else
                        {

                            if (rand.Next(2) == 0)
                            {
                                int a = multiplicity[i, j];
                                int b = multiplicity[i + 1, j];
                                for (int k = 0; k < (int)mode; k++)
                                    if (multiplicity[k, j] == b)
                                        multiplicity[k, j] = a;
                            }
                            else
                            {
                                elements[i + 1, j].left = false;
                                elements[i, j].right = false;
                            }
                        }
                    }
                    bool checkout = true;
                    int mul;
                    bool checkmul = false;
                    while (checkout)
                    {
                        for (int i = 0; i < (int)mode; i++)
                        {
                            if (rand.Next(2) == 0 ||
                                elements[i, j].up && elements[i, j].left && elements[i, j].right)
                            {
                                elements[i, j].down = false;
                                elements[i, j + 1].up = false;
                            }
                            else
                            {
                                elements[i, j].down = true;
                                elements[i, j + 1].up = true;
                            }
                        }
                        mul = multiplicity[0, j];
                        checkmul = false;
                        checkout = false;
                        for (int i = 0; i < (int)mode; i++)
                        {
                            if (mul == multiplicity[i, j])
                            {
                                if (elements[i, j].down)
                                    checkmul = true;
                            }
                            else
                            {
                                if (checkmul)
                                {
                                    mul = multiplicity[i, j];
                                    checkmul = false;
                                    if (elements[i, j].down)
                                        checkmul = true;
                                }
                                else
                                {
                                    checkout = true;
                                    break;
                                }
                            }
                            if (i == (int)mode - 1 && !checkmul)
                                checkout = true;
                        }
                    }

                    for (int i = 0; i < (int)mode; i++)
                    {
                        multiplicity[i, j + 1] = multiplicity[i, j];
                        if (!elements[i, j + 1].up)
                            multiplicity[i, j + 1] = i + (j + 1) * 10;
                    }
                    for (int i = 1; i < (int)mode; i++)
                    {
                        if (multiplicity[i, j + 1] == multiplicity[i - 1, j + 1])
                        {
                            elements[i, j + 1].left = false;
                            elements[i - 1, j + 1].right = false;
                        }
                    }
                }

                for (int i = 0; i < (int)mode - 1; i++)
                {
                    int d = multiplicity[i, (int)mode - 1];
                    for (int j = i + 1; j < (int)mode; j++)
                    {
                        if (multiplicity[j, (int)mode - 1] == d)
                        {
                            elements[j, (int)mode - 1].left = false;
                            elements[j - 1, (int)mode - 1].right = false;
                        }
                    }
                }
            }
        }
    }
}