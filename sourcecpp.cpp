
#include <iostream>

#include <time.h>
#include <conio.h>

static const int FIELD_WIDTH = 12;
static const int FIELD_HEIGHT = 22;

static const int MINO_WIDTH = 4;
static const int MINO_HEIGHT = 4;

enum
{
    MINO_TYPE_I,
    MINO_TYPE_O,
    MINO_TYPE_S,
    MINO_TYPE_Z,
    MINO_TYPE_J,
    MINO_TYPE_L,
    MINO_TYPE_T,
    MINO_TYPE_MAX
};

enum
{
    MINO_ANGLE_0,
    MINO_ANGLE_90,
    MINO_ANGLE_180,
    MINO_ANGLE_270,
    MINO_ANGLE_MAX
};

char field[FIELD_HEIGHT][FIELD_WIDTH] = { 0 };

char displayBuffer[FIELD_HEIGHT][FIELD_WIDTH] = { 0 };

char minoShapes[MINO_TYPE_MAX][MINO_ANGLE_MAX][MINO_WIDTH][MINO_HEIGHT] =
{
    // MINO_TYPE_I
    {
        // MINO_ANGLE_0
        {
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0,
            0, 1, 0, 0
        },
    // MINO_ANGLE_90
    {
        0, 0, 0, 0,
        0, 0, 0, 0,
        1, 1, 1, 1,
        0, 0, 0, 0
    },
    // MINO_ANGLE_180
    {
        0, 0, 1, 0,
        0, 0, 1, 0,
        0, 0, 1, 0,
        0, 0, 1, 0
    },
    // MINO_ANGLE_270
    {
        0, 0, 0, 0,
        1, 1, 1, 1,
        0, 0, 0, 0,
        0, 0, 0, 0
    }
},

// MINO_TYPE_O
{
    // MINO_ANGLE_0
    {
        0, 0, 0, 0,
        0, 1, 1, 0,
        0, 1, 1, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_90
    {
        0, 0, 0, 0,
        0, 1, 1, 0,
        0, 1, 1, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_180
    {
        0, 0, 0, 0,
        0, 1, 1, 0,
        0, 1, 1, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_270
    {
        0, 0, 0, 0,
        0, 1, 1, 0,
        0, 1, 1, 0,
        0, 0, 0, 0
    }
},

// MINO_TYPE_S
{
    // MINO_ANGLE_0
    {
        0, 0, 0, 0,
        0, 1, 1, 0,
        1, 1, 0, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_90
    {
        0, 1, 0, 0,
        0, 1, 1, 0,
        0, 0, 1, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_180
    {
        0, 0, 0, 0,
        0, 1, 1, 0,
        1, 1, 0, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_270
    {
        0, 0, 0, 0,
        0, 1, 0, 0,
        0, 1, 1, 0,
        0, 0, 1, 0
    }
},

// MINO_TYPE_Z
{
    // MINO_ANGLE_0
    {
        0, 0, 0, 0,
        1, 1, 0, 0,
        0, 1, 1, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_90
    {
        0, 0, 0, 0,
        0, 0, 1, 0,
        0, 1, 1, 0,
        0, 1, 0, 0
    },
    // MINO_ANGLE_180
    {
        0, 0, 0, 0,
        0, 1, 1, 0,
        0, 0, 1, 1,
        0, 0, 0, 0
    },
    // MINO_ANGLE_270
    {
        0, 0, 1, 0,
        0, 1, 1, 0,
        0, 1, 0, 0,
        0, 0, 0, 0
    }
},

// MINO_TYPE_J
{
    // MINO_ANGLE_0
    {
        0, 0, 1, 0,
        0, 0, 1, 0,
        0, 1, 1, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_90
    {
        0, 0, 0, 0,
        1, 1, 1, 0,
        0, 0, 1, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_180
    {
        0, 0, 0, 0,
        0, 1, 1, 0,
        0, 1, 0, 0,
        0, 1, 0, 0
    },
    // MINO_ANGLE_270
    {
        0, 0, 0, 0,
        0, 1, 0, 0,
        0, 1, 1, 1,
        0, 0, 0, 0
    }
},

// MINO_TYPE_L
{
    // MINO_ANGLE_0
    {
        0, 1, 0, 0,
        0, 1, 0, 0,
        0, 1, 1, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_90
    {
        0, 0, 0, 0,
        0, 0, 1, 0,
        1, 1, 1, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_180
    {
        0, 0, 0, 0,
        0, 1, 1, 0,
        0, 0, 1, 0,
        0, 0, 1, 0
    },
    // MINO_ANGLE_270
    {
        0, 0, 0, 0,
        0, 1, 1, 1,
        0, 1, 0, 0,
        0, 0, 0, 0
    }
},

// MINO_TYPE_T
{
    // MINO_ANGLE_0
    {
        0, 0, 0, 0,
        1, 1, 1, 0,
        0, 1, 0, 0,
        0, 0, 0, 0
    },
    // MINO_ANGLE_90
    {
        0, 0, 0, 0,
        0, 1, 0, 0,
        0, 1, 1, 0,
        0, 1, 0, 0
    },
    // MINO_ANGLE_180
    {
        0, 0, 0, 0,
        0, 0, 1, 0,
        0, 1, 1, 1,
        0, 0, 0, 0
    },
    // MINO_ANGLE_270
    {
        0, 0, 1, 0,
        0, 1, 1, 0,
        0, 0, 1, 0,
        0, 0, 0, 0
    }
}
};

void display();
bool isHit(int argMinoX, int argMinoY, int argMinoType, int argMinoAngle);
void resetMino();

int minoType = 0;
int minoAngle = 0;
int minoX = 0;
int minoY = 0;

int main()
{
    // create wall and bottom
    for (int i = 0; i < FIELD_HEIGHT; ++i)
    {
        field[i][0] = 1;
        field[i][FIELD_WIDTH - 1] = 1;
    }

    for (int i = 0; i < FIELD_WIDTH; ++i)
    {
        field[FIELD_HEIGHT - 1][i] = 1;
    }

    resetMino();
    time_t t = time(NULL);
    while (1)
    {
        if (_kbhit())
        {
            switch (_getch())
            {
                //case 'w':
            case 's':
                if (!isHit(minoX, minoY + 1, minoType, minoAngle))
                {
                    ++minoY;
                }
                break;
            case 'a':
                if (!isHit(minoX - 1, minoY, minoType, minoAngle))
                {
                    --minoX;
                }
                break;
            case 'd':
                if (!isHit(minoX + 1, minoY, minoType, minoAngle))
                {
                    ++minoX;
                }
                break;
            case 0x20:
                if (!isHit(minoX, minoY, minoType, (minoAngle + 1) % MINO_ANGLE_MAX))
                {
                    minoAngle = (minoAngle + 1) % MINO_ANGLE_MAX;
                }
                break;
            }
            display();
        }

        if (time(NULL) != t)
        {
            t = time(NULL);

            if (isHit(minoX, minoY + 1, minoType, minoAngle))
            {
                for (int i = 0; i < MINO_HEIGHT; ++i)
                {
                    for (int j = 0; j < MINO_WIDTH; ++j)
                    {
                        field[minoY + i][minoX + j] |= minoShapes[minoType][minoAngle][i][j];
                    }
                }

                // erase block
                for (int i = 0; i < FIELD_HEIGHT - 1; ++i)
                {
                    bool isLineFilled = true;
                    for (int j = 1; j < FIELD_WIDTH - 1; ++j)
                    {
                        if (1 != field[i][j])
                        {
                            isLineFilled = false;
                        }
                    }

                    if (true == isLineFilled)
                    {
                        for (int j = i; j > 0; --j)
                        {
                            memcpy(field[j], field[j - 1], FIELD_WIDTH);
                        }
                    }
                }

                resetMino();
            }
            else
            {
                ++minoY;
            }

            display();
        }
    }

    _getch();
    return 0;
}

void display()
{
    // write displayBuffer
    memcpy(displayBuffer, field, sizeof(field));

    for (int i = 0; i < MINO_HEIGHT; ++i)
    {
        for (int j = 0; j < MINO_WIDTH; ++j)
        {
            displayBuffer[minoY + i][minoX + j] |= minoShapes[minoType][minoAngle][i][j];
        }
    }

    // display block on console
    system("cls");

    for (int i = 0; i < FIELD_HEIGHT; ++i)
    {
        for (int j = 0; j < FIELD_WIDTH; ++j)
        {
            if (1 == displayBuffer[i][j])
            {
                std::cout << "¡";
            }
            else
            {
                std::cout << "@";
            }
        }
        std::cout << std::endl;
    }
}

bool isHit(int argMinoX, int argMinoY, int argMinoType, int argMinoAngle)
{
    for (int i = 0; i < MINO_HEIGHT; ++i)
    {
        for (int j = 0; j < MINO_WIDTH; ++j)
        {
            if (minoShapes[argMinoType][argMinoAngle][i][j] && field[argMinoY + i][argMinoX + j])
            {
                return true;
            }
        }
    }
    return false;
}

void resetMino()
{
    minoX = 5;
    minoY = 0;
    minoType = rand() % MINO_TYPE_MAX;
    minoAngle = rand() % MINO_ANGLE_MAX;
}