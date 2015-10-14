using System;
using System.Collections.Generic;

namespace Connect
{
    class Core
    {
        /// <summary>
        /// Клетка в игре
        /// </summary>
        private class Element
        {
            /// <summary>
            /// Численная маска
            /// </summary>
            private int data;

            /// <summary>
            /// Предоставляет доступ к маске элемента
            /// </summary>
            public Mask mask
            {
                get
                {
                    return (Mask)data;
                }
                set
                {
                    data = (int)value;
                }
            }

            /// <summary>
            /// Количество контактов
            /// </summary>
            public int connects
            {
                get
                {
										return (data & 0x1) + (data >> 1 & 0x1) + (data >> 2 & 0x1) + (data >> 3 & 0x1);
                }
            }

            /// <summary>
            /// Сохранение верного решения
            /// </summary>
            public void SaveHint()
            {
                data = data & 0x000f << 8 | data & 0xf0ff;
            }

            /// <summary>
            /// Восстановление верного решения
            /// </summary>
            public void LoadHint()
            {
                data = data & 0x0f00 >> 8 | data & 0xfff0 | (int)Mask.block;
            }

            /// <summary>
            /// Сохранение позиции для последующего восстановления
            /// </summary>
            public void SaveRepeat()
            {
                data = data & 0x000f << 12 | data & 0x0fff;
            }

            /// <summary>
            /// Загрузка сохранённой позиции для повтора игры
            /// </summary>
            public void LoadRepeat()
            {
                data = data & 0xf000 >> 12 | data & 0xff70;
            }

            /// <summary>
            /// Заблокировать | Разблокировать элемент
            /// </summary>
            public void LockUnlock()
            {
								data = data ^ (int)Mask.block;
            }

            /// <summary>
            /// Поворот против часовой стрелке
            /// </summary>
            public void RotationLeft()
            {
                data = data & 0xfff0 | (data >> 1 & 0x007 | data << 3 ) & 0x000f;
            }

            /// <summary>
            /// Поворот по часовой стрелке
            /// </summary>
            public void RotationRight()
            {
                data = data & 0xfff0 | (data << 1 | data >> 3 & 0x0001) & 0x000f;
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

            /// <summary>
            /// Переопределённый оператор равенства
            /// </summary>
            /// <param name="p1"></param>
            /// <param name="p2"></param>
            /// <returns></returns>
            public static bool operator ==(Point p1, Point p2)
            {
                return p1.x == p2.x && p1.y == p2.y;
            }
            /// <summary>
            /// Переопределённый оператор неравенства
            /// </summary>
            /// <param name="p1"></param>
            /// <param name="p2"></param>
            /// <returns></returns>
            public static bool operator !=(Point p1, Point p2)
            {
                return p1.x != p2.x || p1.y != p2.y;
            }
        }

        /// <summary>
        /// Типы сложности игры
        /// </summary>
        public enum Mode { Easy, Normal, Hard, Expert}

        /// <summary>
        /// Тип хода
        /// </summary>
        public enum TypeTurn { left, right, block }

        /// <summary>
        /// Направления
        /// </summary>
        [Flags]
        public enum Mask
        {
            up = 0x0001, right = 0x0002, down = 0x0004, left = 0x0008,
            pc = 0x0010, server = 0x0020, net = 0x0040, block = 0x0080,
            oUp = 0x0100, oRight = 0x0200, oDown = 0x0400, oleft = 0x0800,
            rUp = 0x1000, rRight = 0x2000, rDown = 0x4000, rLeft = 0x8000
        }

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
        private Mask[] sampleElements;

        /// <summary>
        /// Контейнер для хранения уровня сложности
        /// </summary>
        private Mode _mode;
        /// <summary>
        /// Сложность игры
        /// </summary>
        public Mode mode
        {
            get
            {
                return _mode;
            }
            private set
            {
                _mode = value;
                switch (value)
                {
                    case Mode.Easy: width = 5; break;
                    case Mode.Normal: width = 7; break;
                    case Mode.Hard: width = 9; break;
                    case Mode.Expert: width = 9; break;
                }
            }
        }

        public int width { get; private set;  }

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
        public int steps { get; private set; }

        /// <summary>
        /// Количество ходов изначально выделенных для решения
        /// </summary>
        private int rStep;

        /// <summary>
        /// Количество неподключенных ПК
        /// </summary>
        private int countDisconnectedPC;

        /// <summary>
        /// Создание координаты из безмерного поля в реальное
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private int CorrectionCoordinates(int x)
        {
						return x < 0 ? width - 1 + (x + 1) % width : x % width;
        }

        /// <summary>
        /// Предоставляет доступ к элементам поля
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Mask this[int x, int y]
        {
            get
            {
                return elements[CorrectionCoordinates(x), CorrectionCoordinates(y)].mask;
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
                return elements[CorrectionCoordinates(p.x), CorrectionCoordinates(p.y)];
            }
        }

        /// <summary>
        /// Конструктор ядра
        /// </summary>
        public Core()
        {
            history = new List<Turn>();
            sampleElements = new Mask[15];
            for (int i = 1; i < 15; i++)
            {
                sampleElements[i] = (Mask)i;
            }
            elements = new Element[9, 9];
        }

        /// <summary>
        /// Создание новой игры
        /// </summary>
        public void NewGame()
        {
            mode = Mode.Expert;
            CreateField();
        }

        /// <summary>
        /// Создание новой игры с другим уровнем сложности
        /// </summary>
        /// <param name="mode">Уровень сложности</param>
        public void NewGame(Mode mode)
        {
						this.mode = mode;
						CreateField();
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
                    if (!elements[x, y].mask.HasFlag(Mask.block))
                    {
                        elements[x, y].RotationLeft();
                        steps--;
                        CheckConnected();
                    }
                    break;
                case TypeTurn.right:
                    if (!elements[x, y].mask.HasFlag(Mask.block))
                    {
                        elements[x, y].RotationRight();
                        steps--;
                        CheckConnected();
                    }
                    break;
            }
            return countDisconnectedPC == 0;
        }

        public void CallHint(int x, int y)
        {
            elements[x, y].LoadHint();
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
        /// Возвращает соседнюю точку
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private Point BesidePoint(Point p, Mask m)
        {
            switch (m)
            {
                case Mask.up:
                    return new Point(p.x, p.y - 1);
                case Mask.down:
                    return new Point(p.x, p.y + 1);
                case Mask.left:
                    return new Point(p.x - 1, p.y);
                case Mask.right:
                    return new Point(p.x + 1, p.y);
                default:
                    throw new Exception("Передано несколько направлений");
            }
        }

        /// <summary>
        /// Заполнение соседних пиков
        /// </summary>
        /// <param name="p1">Откуда пришёл интернет</param>
        /// <param name="p2">Обрабатываемы пик</param>
        /// <returns></returns>
        private List<Point> SetAround(Point p1, Point p2)
        {
            Point tempPoint;
            List<Point> tempListPoint = new List<Point>();

            for (int i = 1; i < 16; i *= 2)
            {
                if (this[p2].mask.HasFlag((Mask)i))
                {
                    tempPoint = BesidePoint(p2, (Mask)i);
                    if (tempPoint != p1)
                    {
                        tempListPoint.Add(tempPoint);
                        this[tempPoint].mask = (this[tempPoint].mask
                            | (Mask)(((i << 4) | i) >> 2 & 0xf));
                    }
                }
            }

            return tempListPoint;
        }

        /// <summary>
        /// Генерация лабиринта
        /// </summary>
        private void CreateField()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < width; j++)
                {
                    elements[i, j] = new Element();
                }
            Random rand = new Random();
            server = new Point(rand.Next(width), rand.Next(width));
            elements[server.x, server.y].mask = elements[server.x, server.y].mask | Mask.server;

            List<int> availableSamples = new List<int>();

            if (mode == Mode.Expert)
            {
                this[server].mask = this[server].mask | sampleElements[rand.Next(14) + 1];
            }
            else
            {
                for (int i = 1; i < 15; i++)
                {
                    availableSamples.Add(i);
                    for (int j = 1; j < 16; j *= 2)
                    {
                        Point tempPoint = BesidePoint(server, (Mask)j);
                        if ((tempPoint.x < 0
                            || tempPoint.x >= width
                            || tempPoint.y < 0
                            || tempPoint.y >= width)
                            && sampleElements[i].HasFlag((Mask)j))
                        {
                            availableSamples.Remove(i);
                        }
                    }
                }
                this[server].mask = this[server].mask | sampleElements[availableSamples[rand.Next(availableSamples.Count)]];
            }
            List<Point> peaks = SetAround(new Point(-1, -1), server);
            Point peak, netPoint;
            while (peaks.Count > 0)
            {
                peak = peaks[rand.Next(peaks.Count)];
                availableSamples.Clear();

                netPoint = BesidePoint(peak, this[peak].mask);
                for (int i = 1; i < 15; i++)
                {
                    if (sampleElements[i].HasFlag(this[peak].mask))
                    {
                        availableSamples.Add(i);
                    }
                }
                availableSamples.RemoveAt(0);

                /*
                    Данный цикл перебирает стороны по маске и отсекает невозможные пути
                    развития лабиринта, с учётом ячейки из которой он пришёл.
                */
                for (int i = 1; i < 16; i *= 2)
                {
                    Point tempPoint = BesidePoint(peak, (Mask)i);
                    if ((this[tempPoint].connects > 0
                        && !this[peak].mask.HasFlag((Mask)i)) 
                        || (tempPoint.x < 0
                            || tempPoint.x >= width
                            || tempPoint.y < 0
                            || tempPoint.y >= width
                        ) && mode != Mode.Expert
                        )
                    {
                        for (int j = 0; j < 15; j++)
                            if (sampleElements[j].HasFlag((Mask)i))
                                availableSamples.Remove(j);
                    }
                }

                if (availableSamples.Count != 0)
                {
                    this[peak].mask = this[peak].mask | sampleElements[availableSamples[rand.Next(availableSamples.Count)]];
                    peaks.AddRange(SetAround(netPoint, peak));
                }
                peaks.Remove(peak);
            }

            /*
                Инициализация ПК, перемешивание поля, запоминание подсказок,
                сохранение для повтора и расчёт количества ходов.
            */
            steps = 0;
            for (int i = 0; i < width; i++)
                for (int j = 0; j < width; j++)
                {
                    if (elements[i, j].connects == 1 && !elements[i, j].mask.HasFlag(Mask.server))
                    {
                        elements[i, j].mask = elements[i, j].mask | Mask.pc;
                    }
                    elements[i, j].SaveHint();
                    /*if ((int)elements[i, j].mask == 0)
                        continue;
                    switch (rand.Next(4))
                    {
                        case 0:
                            break;
                        case 1:
                            elements[i, j].RotationRight();
                            steps++;
                            break;
                        case 2:
                            if ((int)elements[i, j].mask == 0x000a || (int)elements[i, j].mask == 0x0005)
                                break;
                            elements[i, j].RotationRight();
                            elements[i, j].RotationRight();
                            steps += 2;
                            break;
                        case 3:
                            elements[i, j].RotationLeft();
                            steps++;
                            break;
                    }
                    elements[i, j].SaveRepeat();*/
                }
            rStep = steps;
        }
    }
}