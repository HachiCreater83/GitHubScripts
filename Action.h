#pragma once

class ACTION
{
public:
	void Out()
	{
		Cal();
		Charcter();
	}
private:
	struct
	{
		struct 
		{
			int x = 0;
			int y = POS_MAX_Y - 3 * CELL;
			int Yin = POS_MAX_Y - 3 * CELL;
		}Pos;
	}Player;

	const int MoveX = CELL / 10;
	int Move = 0;
	const int MoveY_max = CELL * 4;

	int Stage_PosX = 0;

	struct 
	{
		bool Xplus = 0;
		bool Xmins = 0;
		bool Jump = 0;
		bool Revece = 0;
	}Flag;

	void Cal();
	void Charcter();
};

ACTION action;

void ACTION::Cal()
{
	if (Key[KEY_INPUT_D] != 0)Flag.Xplus = 1;
	else if (Key[KEY_INPUT_A] != 0)Flag.Xmins = 1;

	if (Key[KEY_INPUT_Y] == 1)
	{
		Flag.Jump = 1;
		Player.Pos.Yin = Player.Pos.y;
	}

	if (Flag.Xplus == 1)
	{
		if (Stage_PosX==0||Stage_PosX==-POS_MAX_X+WIN_MAX_X)
		{
			Player.Pos.x = Player.Pos.x + Move;
		}
		if (Player.Pos.x==WIN_MAX_X/2)
		{
			Stage_PosX = Stage_PosX - Move;
		}
	}
	else if (Flag.Xplus == 1)
	{
		if (Stage_PosX == 0 || Stage_PosX == -POS_MAX_X + WIN_MAX_X)
		{
			Player.Pos.x = Player.Pos.x - Move;
		}
		if (Player.Pos.x == WIN_MAX_X / 2)
		{
			Stage_PosX = Stage_PosX + Move;
		}
	}
}

void ACTION::Charcter()
{
	Stage.Out(&Stage_PosX);
	DrawGraph(Player.Pos.x, Player.Pos.y, Pic.Player, TRUE);

	Flag.Xplus = 0;
	Flag.Xmins = 0;
}

