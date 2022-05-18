using KarenLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace AttackOfTheKarens {
  public partial class FrmMall : Form {
    // consts
    private const int PANEL_PADDING = 10;
    private const int FORM_PADDING = 60;
    private const int CELL_SIZE = 64;
    private readonly Random rand = new Random();
    private readonly Color[] colors = new Color[15] { Color.Bisque, Color.Goldenrod, Color.OldLace, Color.PaleVioletRed, Color.MidnightBlue, Color.Beige,
    Color.Firebrick, Color.GhostWhite, Color.PeachPuff, Color.Gainsboro, Color.Salmon, Color.Thistle, Color.SlateGray, Color.Azure, Color.DarkSeaGreen };
    private Map currentMap = Map.map1;

    // other privates
    private SoundPlayer player;
    private PictureBox picOwner;
    private int xOwner;
    private int yOwner;
    private PictureBox picOwner2;
    private int xOwner2;
    private int yOwner2;
    private PictureBox picOwner3;
    private int xOwner3;
    private int yOwner3;
        private char[][] map;
    private List<Store> stores;
    private string fileContents = File.ReadAllText("data/mall1.txt");
    private bool paused = false;
    private int speed = 250;
    private int bombnum = 0;
    private int xsKaren;
    private int ysKaren;
    private PictureBox sKarenPic;

    enum Map
        {
            map1,
            map2,
            map3,
            map4,
            map5,
            map1p,
            map2p,
            map3p,
            map4p,
            map5p,
            map1p2,
            map2p2,
            map3p2,
            map4p2,
            map5p2,
            mapwin
        }

        // ctor
        public FrmMall() {
      Game.openForms.Add(this);
      InitializeComponent();
    }

        // functions

        private void panMall_MouseDown(object sender, MouseEventArgs e)
        {
            
        }
        private void LoadMap(string fc) {
      string[] lines = fc.Split(Environment.NewLine);
      map = new char[lines.Length][];
      for (int i = 0; i < lines.Length; i++) {
        map[i] = lines[i].ToCharArray();
      }
    }

    private PictureBox CreatePic(Image img, int top, int left) {
      return new PictureBox() {
        Image = img,
        Top = top,
        Left = left,
        Width = CELL_SIZE,
        Height = CELL_SIZE,
      };
    }

    private PictureBox CreateWall(Color color, Image img, int top, int left) {
      PictureBox picWall = CreatePic(img, top, left);
      picWall.Image.Tint(color);
      return picWall;
    }

    private void GenerateMall(Color color) {
      panMall.Controls.Clear();
      int top = 0;
      int left = 0;

      PictureBox pic = null;
      foreach (char[] array in map) {
        foreach (char c in array) {
          switch (c) {
            case 'K':
              pic = CreatePic(Properties.Resources.karen, top, left);
              pic = CreatePic(Properties.Resources.karen, top, left);
              Store s = new Store(new Karen(pic) {
                Row = top / CELL_SIZE,
                Col = left / CELL_SIZE,
              });
              stores.Add(s);
              break;
            case 'o':
                int n = rand.Next(4);
                switch (n)
                {
                    case 0:
                        picOwner = CreatePic(Properties.Resources.owner_pi, top, left);
                        break;
                    case 1:
                        picOwner = CreatePic(Properties.Resources.owner_w, top, left);
                        break;
                    case 2:
                        picOwner = CreatePic(Properties.Resources.owner_p, top, left);
                        break;
                    case 3:
                        picOwner = CreatePic(Properties.Resources.owner_r, top, left);
                        break;
                }
                xOwner = left / CELL_SIZE;
                yOwner = top / CELL_SIZE;
                panMall.Controls.Add(picOwner);
                break;
                        case 'w': pic = CreatePic(Properties.Resources.water, top, left); break;
            case '-': pic = CreateWall(color, Properties.Resources.hline, top, left); break;
            case '|': pic = CreateWall(color, Properties.Resources.vline, top, left); break;
            case 'a': pic = CreateWall(color, Properties.Resources.a, top, left); break;
            case 'b': pic = CreateWall(color, Properties.Resources.b, top, left); break;
            case 'c': pic = CreateWall(color, Properties.Resources.c, top, left); break;
            case 'd': pic = CreateWall(color, Properties.Resources.d, top, left); break;
            case 'e': pic = CreateWall(color, Properties.Resources.e, top, left); break;
            case 'f': pic = CreateWall(color, Properties.Resources.f, top, left); break;
            case 'g': pic = CreateWall(color, Properties.Resources.g, top, left); break;
            case 'h': pic = CreateWall(color, Properties.Resources.h, top, left); break;
            case 'm':
                picOwner2 = CreatePic(Properties.Resources.owner, top, left);
                xOwner2 = left / CELL_SIZE;
                yOwner2 = top / CELL_SIZE;
                panMall.Controls.Add(picOwner2);
                picOwner2.BringToFront();
                break;
            case 'S':
                pic = CreatePic(Properties.Resources.superkaren, top, left);
                s = new Store(new Karen(pic)
                {
                    Row = top / CELL_SIZE,
                    Col = left / CELL_SIZE,
                    Health = 3,
                });
                stores.Add(s);
                break;
            case 'z':
                picOwner3 = CreatePic(Properties.Resources.owner, top, left);
                xOwner3 = left / CELL_SIZE;
                yOwner3 = top / CELL_SIZE;
                panMall.Controls.Add(picOwner3);
                picOwner3.BringToFront();
                            break;
                    }
          left += CELL_SIZE;
          if (pic != null) {
            panMall.Controls.Add(pic);
          }
        }
        left = 0;
        top += CELL_SIZE;
      }


      picOwner.BringToFront();
      panMall.Width = CELL_SIZE * map[0].Length + PANEL_PADDING;
      panMall.Height = CELL_SIZE * map.Length + PANEL_PADDING;
      this.Width = panMall.Width + FORM_PADDING + 75;
      this.Height = panMall.Height + FORM_PADDING;
      lblMoneySaved.Left = this.Width - lblMoneySaved.Width - 10;
      lblMoneySavedLabel.Left = this.Width - lblMoneySavedLabel.Width - 10;
      lblMoneySavedLabel.Top = 0;
      lblMoneySaved.Top = lblMoneySavedLabel.Height + 5;

            button1.Text = "Speed Up";
            button2.Text = "Bomb";
            button3.Text = "Horde";
            button4.Text = "Next Stage";
            button5.Text = "Pause";

            button1.Location = new Point(1369, 100);
            button2.Location = new Point(1369, 150);
            button3.Location = new Point(1369, 200);
            button4.Location = new Point(1369, 250);
            button5.Location = new Point(1369, 750);
        }

    private void FrmMall_Load(object sender, EventArgs e) {
      stores = new List<Store>();
      LoadMap(fileContents);
      GenerateMall(colors[rand.Next(colors.Length)]);
      tmrKarenSpawner.Interval = rand.Next(1000, 5000);
      tmrKarenSpawner.Enabled = true;
      tmrMoveOwner.Interval = 250;
      timer1.Interval = 250;
      timer3.Interval = 250;
            if (currentMap == Map.map1 || currentMap == Map.map1p || currentMap == Map.map1p2)
            {
                player = new SoundPlayer();
                player.SoundLocation = "data/bang.wav";
                player.PlayLooping();
            }
    }

    private bool IsInBounds(int newRow, int newCol) {
      return (newRow >= 0 && newRow < map.Length && newCol >= 0 && newCol < map[0].Length);
    }

    private bool IsWalkable(int newRow, int newCol) {
      char[] walkableTiles = new char[] { ' ', 'o', 'K', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'L', 'm', 'z'};
      return walkableTiles.Contains(map[newRow][newCol]);
    }

    private bool CanMove(Direction dir, out int newRow, out int newCol) {
      newRow = yOwner;
      newCol = xOwner;
      switch (dir) {
        case Direction.UP: newRow--; break;
        case Direction.DOWN: newRow++; break;
        case Direction.LEFT: newCol--; break;
        case Direction.RIGHT: newCol++; break;
      }
      return (IsInBounds(newRow, newCol) && IsWalkable(newRow, newCol));
    }

    private bool CanMove2(Direction dir, out int newRow, out int newCol)
    {
        newRow = yOwner2;
        newCol = xOwner2;
        switch (dir)
        {
            case Direction.UP: newRow--; break;
            case Direction.DOWN: newRow++; break;
            case Direction.LEFT: newCol--; break;
            case Direction.RIGHT: newCol++; break;
        }
        return (IsInBounds(newRow, newCol) && IsWalkable(newRow, newCol));
    }

    private bool CanMove3(Direction dir, out int newRow, out int newCol)
    {
        newRow = yOwner3;
        newCol = xOwner3;
        switch (dir)
        {
            case Direction.UP: newRow--; break;
            case Direction.DOWN: newRow++; break;
            case Direction.LEFT: newCol--; break;
            case Direction.RIGHT: newCol++; break;
        }
        return (IsInBounds(newRow, newCol) && IsWalkable(newRow, newCol));
    }

        private new void Move(Direction dir) {
      if (CanMove(dir, out int newRow, out int newCol)) {
        yOwner = newRow;
        xOwner = newCol;
        picOwner.Top = yOwner * CELL_SIZE;
        picOwner.Left = xOwner * CELL_SIZE;
        char mapTile = map[newRow][newCol];
        switch (mapTile) {
          case '0':
          case '1':
          case '2':
          case '3':
          case '4':
          case '5':
          case '6':
          case '7':
          case '8':
          case '9':
            stores[int.Parse(mapTile.ToString())].OwnerWalksIn();
            break;
          case 'L':
            foreach (Store store in stores) {
              store.ResetOwner();
            }
            break;
        }
      }
    }

        private new void Move2(Direction dir)
        {
            if (CanMove2(dir, out int newRow, out int newCol))
            {
                yOwner2 = newRow;
                xOwner2 = newCol;
                picOwner2.Top = yOwner2 * CELL_SIZE;
                picOwner2.Left = xOwner2 * CELL_SIZE;
                char mapTile = map[newRow][newCol];
                switch (mapTile)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        stores[int.Parse(mapTile.ToString())].OwnerWalksIn();
                        break;
                    case 'L':
                        foreach (Store store in stores)
                        {
                            store.ResetOwner();
                        }
                        break;
                }
            }
        }

        private new void Move3(Direction dir)
        {
            if (CanMove3(dir, out int newRow, out int newCol))
            {
                yOwner3 = newRow;
                xOwner3 = newCol;
                picOwner3.Top = yOwner3 * CELL_SIZE;
                picOwner3.Left = xOwner3 * CELL_SIZE;
                char mapTile = map[newRow][newCol];
                switch (mapTile)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        stores[int.Parse(mapTile.ToString())].OwnerWalksIn();
                        break;
                    case 'L':
                        foreach (Store store in stores)
                        {
                            store.ResetOwner();
                        }
                        break;
                }
            }
        }

        private void FrmMall_KeyUp(object sender, KeyEventArgs e) {
      switch (e.KeyCode) {
        case Keys.Up: Move(Direction.UP); break;
        case Keys.Down: Move(Direction.DOWN); break;
        case Keys.Left: Move(Direction.LEFT); break;
        case Keys.Right: Move(Direction.RIGHT); break;
      }
    }

    private void tmrKarenSpawner_Tick(object sender, EventArgs e) {
            if (currentMap != Map.mapwin)
            {
                Store s = stores[rand.Next(stores.Count)];
                s.ActivateTheKaren();
            }
    }

    private void FrmMall_FormClosed(object sender, FormClosedEventArgs e) {
      Game.openForms.Remove(this);
      Game.CloseAll();
    }

    private void tmrUpdateKarens_Tick(object sender, EventArgs e) {
      if (stores != null && stores.Count > 0) {
        foreach (Store store in stores) {
          store.Update();
        }
      }
    }

    private void tmrMoveOwner_Tick(object sender, EventArgs e) {
      Direction dir = (Direction)rand.Next(4);
      Move(dir);
    }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Direction dir = (Direction)rand.Next(4);
            Move2(dir);
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if(timer2.Interval > 1000)
            {
                timer2.Interval = 1000;
            }
            Game.SubFromScore((float).01);
        }

        private void tmrUpdateGame_Tick(object sender, EventArgs e) {
      lblMoneySaved.Text = Game.Score.ToString("$ #,##00.00");
    }
        // speed up
        private void button1_Click(object sender, EventArgs e)
        {
            float money = Game.CheckScore();
            if(money >= 10){
                if(tmrMoveOwner.Interval >= 10)
                {
                    Game.SubFromScore(10);
                    tmrMoveOwner.Interval -= 10;
                    speed = tmrMoveOwner.Interval;
                }
            }
        }

        // bomb
        private void button2_Click(object sender, EventArgs e)
        {
            float money = Game.CheckScore();
            
            if(money >= 30){
                Game.SubFromScore(30);
                foreach (Store store in stores) {
                    store.BombUsed();
                    store.Update();
                    store.BombFinished();
                }
                if (currentMap == Map.map5)
                {
                    bombnum++;
                    if(bombnum == 1)
                    {
                        sKarenPic = CreatePic(Properties.Resources.superkaren_d1, 685, 375);
                        sKarenPic.BringToFront();
                    }
                    if (bombnum >= 4)
                    {
                        tmrMoveOwner.Interval = 250;
                        timer1.Interval = 250;
                        bombnum = 0;
                        tmrKarenSpawner.Interval = 999999999;
                        Game.AddToScore(100);
                        currentMap = Map.map1p;
                        fileContents = File.ReadAllText("data/mall1p1.txt");
                        FrmMall_Load(null, null);
                    }
                }

                if(currentMap == Map.map5p)
                {
                    bombnum++;
                    if (bombnum >= 4)
                    {
                        tmrKarenSpawner.Interval = 999999999;
                        Game.AddToScore(100);
                        tmrMoveOwner.Interval = 250;
                        timer1.Interval = 250;
                        bombnum = 0;
                        currentMap = Map.map1p2;
                        fileContents = File.ReadAllText("data/mall1p2.txt");
                        FrmMall_Load(null, null);
                    }
                }
                if(currentMap == Map.map5p2)
                {
                    bombnum++;
                    if (bombnum >= 4)
                    {
                        button1.Enabled = false;
                        button2.Enabled = false;
                        button3.Enabled = false;
                        button4.Enabled = false;
                        button5.Enabled = false;
                        timer2.Enabled = false;
                        tmrKarenSpawner.Interval = 999999999;
                        tmrMoveOwner.Interval = 999999999;
                        timer1.Interval = 999999999;
                        currentMap = Map.mapwin;
                        fileContents = File.ReadAllText("data/mallwin.txt");
                        FrmMall_Load(null, null);
                    }
                }
            }

        }

        // horde
        private void button3_Click(object sender, EventArgs e)
        {
            float money = Game.CheckScore();
            if(money >= 20){
                Game.SubFromScore(20);
                foreach (Store store in stores)
                {
                    store.HordeStart();
                    store.Update();
                    store.HordeEnd();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            float money = Game.CheckScore();
            if (money >= 50 && (currentMap != Map.map5 || currentMap != Map.map5p || currentMap != Map.map5p2))
            {
                Game.SubFromScore(50);
                if (currentMap == Map.map1)
                {
                    currentMap = Map.map2;
                    fileContents = File.ReadAllText("data/mall2.txt");
                    FrmMall_Load(null, null);

                }
                else if (currentMap == Map.map2)
                {
                    currentMap = Map.map3;
                    fileContents = File.ReadAllText("data/mall3.txt");
                    FrmMall_Load(null, null);
                }
                else if (currentMap == Map.map3)
                {
                    currentMap = Map.map4;
                    fileContents = File.ReadAllText("data/mall4.txt");
                    FrmMall_Load(null, null);
                }
                else if (currentMap == Map.map4)
                {
                    currentMap = Map.map5;
                    fileContents = File.ReadAllText("data/mall5.txt");
                    FrmMall_Load(null, null);
                }
                else if (currentMap == Map.map1p)
                {
                    currentMap = Map.map2p;
                    fileContents = File.ReadAllText("data/mall2p1.txt");
                    FrmMall_Load(null, null);
                }
                else if (currentMap == Map.map2p)
                {
                    currentMap = Map.map3p;
                    fileContents = File.ReadAllText("data/mall3p1.txt");
                    FrmMall_Load(null, null);
                }
                else if (currentMap == Map.map3p)
                {
                    currentMap = Map.map4p;
                    fileContents = File.ReadAllText("data/mall4p1.txt");
                    FrmMall_Load(null, null);
                }
                else if (currentMap == Map.map4p)
                {
                    currentMap = Map.map5p;
                    fileContents = File.ReadAllText("data/mall5p1.txt");
                    FrmMall_Load(null, null);
                }
                else if (currentMap == Map.map1p2)
                {
                    currentMap = Map.map2p2;
                    fileContents = File.ReadAllText("data/mall2p2.txt");
                    FrmMall_Load(null, null);
                }
                else if (currentMap == Map.map2p2)
                {
                    currentMap = Map.map3p2;
                    fileContents = File.ReadAllText("data/mall3p2.txt");
                    FrmMall_Load(null, null);
                }
                else if (currentMap == Map.map3p2)
                {
                    currentMap = Map.map4p2;
                    fileContents = File.ReadAllText("data/mall4p2.txt");
                    FrmMall_Load(null, null);
                }
                else if (currentMap == Map.map4p2)
                {
                    currentMap = Map.map5p2;
                    fileContents = File.ReadAllText("data/mall5p2.txt");
                    FrmMall_Load(null, null);
                }
            }
        }

        private void FrmMall_Click(object sender, EventArgs e)
        {

        }

        private void panMall_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                yOwner = e.Y;
                xOwner = e.X;
                picOwner.Top = yOwner;
                picOwner.Left = xOwner;
                yOwner = yOwner / CELL_SIZE;
                xOwner = xOwner / CELL_SIZE;
                foreach (Store store in stores)
                {
                    store.ResetOwner();
                }

                char mapTile = map[yOwner][xOwner];
                switch (mapTile)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        stores[int.Parse(mapTile.ToString())].OwnerWalksIn();
                        break;
                    case 'L':
                        foreach (Store store in stores)
                        {
                            store.ResetOwner();
                        }
                        break;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!paused)
            {
                tmrKarenSpawner.Interval = 999999999;
                tmrMoveOwner.Interval = 999999999;
                timer1.Interval = 999999999;
                paused = true;
            }
            else
            {
                tmrKarenSpawner.Interval = rand.Next(1000, 5000); ;
                tmrMoveOwner.Interval = speed;
                timer1.Interval = speed;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Direction dir = (Direction)rand.Next(4);
            Move3(dir);
        }
    }
}
