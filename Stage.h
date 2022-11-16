#pragma once

class STAGE
{
public:

	//Block type
	struct
	{
		int Type[STAGE_MAX_X][STAGE_MAX_Y];
		int Type_Exp[POS_MAX_X][POS_MAX_Y];
	}Block;

	void Read()
	{
		FILE* fp_stage_1_1;
		fp_stage_1_1 = fopen("stage_1_1.txt", "r");
		//'r'=read

		for (int y = 0; y < STAGE_MAX_Y; y = y++)
		{
			for (int x = 0; x < STAGE_MAX_X; x = x++)
			{
				(void)fscanf(fp_stage_1_1, "%d", &Block.Type[x][y]);
			}
		}
		fclose(fp_stage_1_1);
	}

	void Out(int* PosX)
	{
		int pic = 0;

		for (int x = 0; x < STAGE_MAX_X; x = x++)
		{
			for (int y = 0; y < STAGE_MAX_Y; y = y++)
			{
				switch (Block.Type[x][y])
				{
				case 0:
					pic = Pic.Back;
					break;
				case 1:
					pic = Pic.Block1;
					break;
				case 2:
					pic = Pic.Block2;
					break;
				case 3:
					pic = Pic.Block3;
					break;
				case 4:
					pic = Pic.Block3;
					break;
				case 5:
					pic = Pic.Block3;
					break;
				}
				DrawGraph(CELL * x + *PosX, CELL * y, pic, TRUE);
			}
		}

	}
private:
};

STAGE Stage;
