namespace WargameExplainer.SampleGames.OthelloBit;

public static class FlipBit
{
    // Optimized for x64 architecture

    // array with flip functions, includes specific bit pattern for every position
    public static Func<ulong, ulong, ulong>[] flipOperations =
    {
        Flip_A1, Flip_B1, Flip_C1, Flip_D1, Flip_E1, Flip_F1, Flip_G1, Flip_H1,
        Flip_A2, Flip_B2, Flip_C2, Flip_D2, Flip_E2, Flip_F2, Flip_G2, Flip_H2,
        Flip_A3, Flip_B3, Flip_C3, Flip_D3, Flip_E3, Flip_F3, Flip_G3, Flip_H3,
        Flip_A4, Flip_B4, Flip_C4, Flip_D4, Flip_E4, Flip_F4, Flip_G4, Flip_H4,
        Flip_A5, Flip_B5, Flip_C5, Flip_D5, Flip_E5, Flip_F5, Flip_G5, Flip_H5,
        Flip_A6, Flip_B6, Flip_C6, Flip_D6, Flip_E6, Flip_F6, Flip_G6, Flip_H6,
        Flip_A7, Flip_B7, Flip_C7, Flip_D7, Flip_E7, Flip_F7, Flip_G7, Flip_H7,
        Flip_A8, Flip_B8, Flip_C8, Flip_D8, Flip_E8, Flip_F8, Flip_G8, Flip_H8
    };


    public static ulong Flip_A1(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0101010101010100: all bits vertical from A2 to A8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0101010101010100ul);

        // bit mask 0x8040201008040200: all bits diagonal from B2 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x8040201008040200ul);

        // bit mask 0x00000000000000FE: all bits horizontal from B1 to H1
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000000000FEul);

        return flipped;
    }


    public static ulong Flip_B1(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0202020202020200: all bits vertical from B2 to B8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0202020202020200ul);

        // bit mask 0x0080402010080400: all bits diagonal from C2 to H7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0080402010080400ul);

        // bit mask 0x00000000000000FC: all bits horizontal from C1 to H1
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000000000FCul);

        return flipped;
    }


    public static ulong Flip_C1(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0404040404040400: all bits vertical from C2 to C8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0404040404040400ul);

        // bit mask 0x0000000000010200: all bits diagonal from B2 to A3
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000000010200ul);

        // bit mask 0x0000804020100800: all bits diagonal from D2 to H6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000804020100800ul);

        // bit mask 0x00000000000000F8: all bits horizontal from D1 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000000000f8ul);

        // bits mask 0x0000000000000003: all bits horizontal from B1 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000003ul);

        return flipped;
    }


    public static ulong Flip_D1(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0808080808080800: all bits vertical from D2 to D8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0808080808080800ul);

        // bit mask 0x0000000001020400: all bits diagonal from C2 to A4
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000001020400ul);

        // bit mask 0x0000008040201000: all bits diagonal from E2 to H5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000008040201000ul);

        // bit mask 0x00000000000000F0: all bits diagonal from E1 to H1
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000000000F0ul);

        // bit mask 0x0000000000000007: all bits horizontal from C1 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000007ul);

        return flipped;
    }


    public static ulong Flip_E1(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x1010101010101000: all bits vertical from E2 to E8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x1010101010101000ul);

        // bit mask 0x0000000102040800: all bits diagonal from D2 to A5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000102040800ul);

        // bit mask 0x0000000080402000: all bits diagonal from F2 to H4
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000080402000ul);

        // bit mask 0x00000000000000E0 all bits horizontal from F1 to H1
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000000000E0ul);

        // bit mask 0x000000000000000F: all bits horizontal from D1 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x000000000000000Ful);

        return flipped;
    }


    public static ulong Flip_F1(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x2020202020202000: all bits vertical from F2 to F8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x2020202020202000ul);

        // bit mask 0x0000010204081000: all bits diagonal from E2 to A6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000010204081000ul);

        // bit mask 0x0000000000804000: all bits diagonal from G2 to H3
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000000804000ul);

        // bit mask 0x00000000000000C0: all bits horizontal from G1 to H1
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000000000C0ul);

        // bit mask 0x000000000000001F: all bits horizontal from E1 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x000000000000001Ful);

        return flipped;
    }


    public static ulong Flip_G1(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x4040404040404000: all bits vertical from G2 to G8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x4040404040404000ul);

        // bit mask 0x0001020408102000: all bits diagonal from F2 to A7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0001020408102000ul);

        // bit mask 0x000000000000003F: all bits horizontal from F1 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x000000000000003Ful);

        return flipped;
    }


    public static ulong Flip_H1(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x8080808080808000: all bits vertical from H2 to H8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x8080808080808000ul);

        // bit mask 0x0102040810204000: all bits diagonal from G2 to A8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0102040810204000ul);

        // bit mask right: all bits horizontal from G1 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x000000000000007Ful);

        return flipped;
    }


    public static ulong Flip_A2(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0101010101010000: all bits vertical from A3 to A8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0101010101010000ul);

        // bit mask 0x4020100804020000: all bits diagonal from B3 to G8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x4020100804020000ul);

        // bit mask 0x000000000000FE00: all bits horizontal from B2 to H2
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000000000FE00ul);

        return flipped;
    }


    public static ulong Flip_B2(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0202020202020000: all bits vertical from B3 to B8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0202020202020000ul);

        // bit mask 0x8040201008040000: all bits diagonal from C3 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x8040201008040000ul);

        // bit mask 0x000000000000FC00: all bits horizontal from C2 to H2
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000000000FC00ul);

        return flipped;
    }


    public static ulong Flip_C2(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0404040404040000: all bits vertical from C3 to C8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0404040404040000ul);

        // bit mask 0x0000000001020000: all bits diagonal from B3 to A4
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000001020000ul);

        // bit mask 0x0080402010080000: all bits diagonal from D3 to H7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0080402010080000ul);

        // bit mask 0x000000000000F800: all bits horizontal from D2 to H2
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000000000F800ul);

        // bit mask 0x0000000000000300: all bits horizontal from B2 to A2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000300ul);

        return flipped;
    }


    public static ulong Flip_D2(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0808080808080000: all bits vertical from D3 to D8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0808080808080000ul);

        // bit mask 0x0000000102040000: all bits diagonal from C3 to A5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000102040000ul);

        // bit mask 0x0000804020100000: all bits diagonal from E3 to H6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000804020100000ul);

        // bit mask 0x000000000000F000: all bits horizontal from E2 to H2
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000000000F000ul);

        // bit mask 0x0000000000000700: all bits horizontal from C2 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000700ul);

        return flipped;
    }


    public static ulong Flip_E2(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x1010101010100000: all bits vertical from E3 to E8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x1010101010100000ul);

        // bit mask 0x0000010204080000: all bits diagonal from D3 to A6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000010204080000ul);

        // bit mask 0x0000008040200000: all bits diagonal from F3 to H5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000008040200000ul);

        // bit mask 0x000000000000E000: all bits horizontal from F2 to H2
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000000000E000ul);

        // bit mask 0x0000000000000F00: all bits horizontal from D2 to A2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000F00ul);

        return flipped;
    }


    public static ulong Flip_F2(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x2020202020200000: all bits vertical from F3 to F8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x2020202020200000ul);

        // bit mask 0x0000000080400000: all bits diagonal from G3 to H4
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000080400000ul);

        // bit mask 0x0001020408100000: all bits diagonal from E3 to A7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0001020408100000ul);

        // bit mask 0x000000000000C000: all bits horizontal from G2 to H2
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000000000C000ul);

        // bit mask 0x0000000000001F00: all bits horizontal from E2 to A2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000001F00ul);

        return flipped;
    }


    public static ulong Flip_G2(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x4040404040400000: all bits vertical from G3 to G8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x4040404040400000ul);

        // bit mask 0x0102040810200000: all bits diagonal from F3 to A8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0102040810200000ul);

        // bit mask 0x0000000000003F00: all bits horizontal from F2 to A2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000003F00ul);

        return flipped;
    }


    public static ulong Flip_H2(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x8080808080800000: all bits vertical from H3 to H8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x8080808080800000ul);

        // bit mask 0x0204081020400000: all bits vertical from G3 to B8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0204081020400000ul);

        // bit mask 0x0000000000007F00: all bits vertical from G2 to A2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000007F00ul);

        return flipped;
    }


    public static ulong Flip_A3(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0101010101000000: all bits vertical from A4 to A8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0101010101000000ul);

        // bit mask 0x0000000000000101: all bits vertical from A2 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000101ul);

        // bit mask 0x2010080402000000: all bits diagonal from B4 to F8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x2010080402000000ul);

        // bis mask 0x0000000000000204: all bits diagonal from B2 to C1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000204ul);

        // bit mask 0x0000000000FE0000: all bits horizontal from B3 to H3
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000000FE0000ul);

        return flipped;
    }


    public static ulong Flip_B3(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0202020202000000: all bits vertical from B4 to B8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0202020202000000ul);

        // bit mask 0x0000000000000202: all bits vertical from B2 to B1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000202ul);

        // bit mask 0x4020100804000000: all bits diagonal from C4 to G1
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x4020100804000000ul);

        // bit mask 0x0000000000000408: all bits diagonal from C2 to D1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000408ul);

        // bit mask 0x0000000000FC0000: all bits diagonal from C3 to H3
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000000FC0000ul);

        return flipped;
    }


    public static ulong Flip_C3(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0404040404000000: all bits vertical from C4 to C8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0404040404000000ul);

        // bit mask 0x0000000000000404: all bits vertical from C2 to C1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000404ul);

        // bit mask 0x0000000102000000: all bits diagonal from B4 to A5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000102000000ul);

        // bit mask 0x8040201008000000: all bits diagonal from D4 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x8040201008000000ul);

        // bit mask 0x0000000000000201: all bits diagonal from B2 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000201ul);

        // bit mask 0x0000000000000810: all bits diagonal from D2 to E1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000810ul);

        // bit mask 0x0000000000F80000: all bits horizontal from D3 to H3
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000000F80000ul);

        // bit mask 0x0000000000030000: all bits horizontal from B3 to A3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000030000ul);

        return flipped;
    }


    public static ulong Flip_D3(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0808080808000000: all bits vertical from D4 to D8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0808080808000000ul);

        // bit mask 0x0000000000000808: all bits vertical from D2 to D1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000808ul);

        // bit mask 0x0000010204000000: all bits diagonal from C4 to A6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000010204000000ul);

        // bit mask 0x0080402010000000: all bits diagonal from D3 to H7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0080402010000000ul);

        // bit mask 0x0000000000000402: all bits diagonal from C2 to B1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000402ul);

        // bit mask 0x0000000000001020: all bits diagonal from E2 to F1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000001020ul);

        // bit mask 0x0000000000F00000: all bits horizontal from E3 to H3
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000000F00000ul);

        // bit mask 0x0000000000070000: all bits horizontal from C3 to A3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000070000ul);

        return flipped;
    }


    public static ulong Flip_E3(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x1010101010000000: all bits vertical from E4 to E8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x1010101010000000ul);

        // bit mask 0x0000000000001010: all bits vertical from E2 to E1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000001010ul);

        // bit mask 0x0001020408000000: all bits diagonal from E3 to A7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0001020408000000ul);

        // bit mask 0x0000804020000000: all bits diagonal from F4 to H6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000804020000000ul);

        // bit mask 0x0000000000000804: all bits diagonal from D2 to C1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000000804ul);

        // bit mask 0x0000000000002040: all bits diagonal from F2 to G1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000002040ul);

        // bit mask 0x0000000000E00000: all bits horizontal from F3 to H3
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000000E00000ul);

        // bit mask 0x00000000000F0000: all bits horizontal from D3 to A3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x00000000000F0000ul);

        return flipped;
    }


    public static ulong Flip_F3(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x2020202020000000: all bits vertical from F4 to F8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x2020202020000000ul);

        // bit mask 0x0000000000002020: all bits vertical from F2 to F1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000002020ul);

        // bit mask 0x0102040810000000: all bits diagonal from E4 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0102040810000000ul);

        // bit mask 0x0000008040000000ul: all bits diagonal from G4 to H5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000008040000000ul);

        // bit mask 0x0000000000001008: all bits diagonal from E2 to D1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000001008ul);

        // bit mask 0x0000000000004080: all bits diagonal from G2 to H1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000004080ul);

        // bit mask 0x0000000000C00000: all bits horizontal from G3 to H3
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000000000C00000ul);

        // bit mask 0x00000000001F0000: all bits horizontal from E3 to A3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x00000000001F0000ul);

        return flipped;
    }


    public static ulong Flip_G3(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x4040404040000000: all bits vertical from G4 to G8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x4040404040000000ul);

        // bit mask 0x0000000000004040: all bits vertical from G2 to G1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000004040ul);

        // bit mask 0x0204081020000000: all bits diagonal from F4 to B1
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0204081020000000ul);

        // bit mask 0x0000000000002010: all bits diagonal from C2 to D1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000002010ul);

        // bit mask 0x00000000003F0000: all bits horizontal from F3 to A3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x00000000003F0000ul);

        return flipped;
    }


    public static ulong Flip_H3(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x8080808080000000: all bits vertical from H4 to H8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x8080808080000000ul);

        // bit mask 0x0000000000008080: all bits vertical from H2 to H1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000008080ul);

        // bit mask 0x0408102040000000: all bits diagonal from G4 to C8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0408102040000000ul);

        // bit mask 0x0000000000004020: all bits diagonal from G2 to F1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000004020ul);


        // bit mask 0x00000000007F0000: all bits vertical from G3 to A3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x00000000007F0000ul);

        return flipped;
    }


    public static ulong Flip_A4(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0101010100000000: all bits vertical from A5 to A8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0101010100000000ul);

        // bit mask 0x0000000000010101: all bits vertical from A3 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000010101ul);

        // bit mask 0x1008040200000000: fiels diagonal from B5 to E8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x1008040200000000ul);

        // bis mask 0x0000000000020408: all bits diagonal from B3 to D1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000020408ul);

        // bit mask 0x00000000FE000000: all bits horizontal from B4 to H4
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000FE000000ul);

        return flipped;
    }


    public static ulong Flip_B4(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0202020200000000: all bits vertical from B5 to B8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0202020200000000ul);

        // bit mask 0x0000000000020202: all bits vertical from B3 to B1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000020202ul);

        // bit mask 0x2010080400000000: all bits diagonal fromn C5 to F6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x2010080400000000ul);

        // bit mask 0x0000000000040810: all bits diagonal from C3 to E1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000040810ul);

        // bit mask 0x00000000FC000000: all bits diagonal from C4 to H4
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000FC000000ul);

        return flipped;
    }


    public static ulong Flip_C4(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0404040400000000: all bits vertical from C5 to C8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0404040400000000ul);

        // bit mask 0x0000000000040404: all bits vertical from C3 to C1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000040404ul);

        // bit mask 0x0000010200000000: all bits diagonal from B5 to A6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000010200000000ul);

        // bit mask 0x4020100800000000: all bits diagonal from D5 to G8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x4020100800000000ul);

        // bit mask 0x0000000000020100: all bits diagonal from B3 to A2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000020100ul);

        // bit mask 0x0000000000081020: all bits diagonal from D3 to F1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000081020ul);

        // bit mask 0x00000000F8000000: all bits horizontal from D4 to H4
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000F8000000ul);

        // bit mask 0x0000000003000000: all bits horizontal from B4 to A4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000003000000ul);

        return flipped;
    }


    public static ulong Flip_D4(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0808080800000000: all bits vertical from D5 to D8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0808080800000000ul);

        // bit mask 0x0000000000080808: all bits vertical from D3 to D1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000080808ul);

        // bit mask 0x0001020400000000: all bits diagonal from C5 to A7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0001020400000000ul);

        // bit mask 0x8040201000000000: all bits diagonal from E5 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x8040201000000000ul);

        // bit mask 0x0000000000040201: all bits diagonal from C3 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000040201ul);

        // bit mask 0x0000000000102040: all bits diagonal from E3 to G1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000102040ul);

        // bit mask 0x00000000F0000000: all bits horizontal from E4 to H4
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000F0000000ul);

        // bit mask 0x0000000007000000: all bits horizontal from C4 to A4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000007000000ul);

        return flipped;
    }


    public static ulong Flip_E4(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x1010101000000000: all bits vertical from E5 to E8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x1010101000000000ul);

        // bit mask 0x0000000000101010: all bits vertical from E3 to E1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000101010ul);

        // bit mask 0x0102040800000000: all bits diagonal from D5 to A8 
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0102040800000000ul);

        // bit mask 0x0080402000000000: all bits diagonal from F5 to H7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0080402000000000ul);

        // bit mask 0x0000000000080402: all bits diagonal from D3 to B1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000080402ul);

        // bit mask 0x0000000000204080: all bits diagonal from F3 to H1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000204080ul);

        // bit mask 0x00000000E0000000: all bits horizontal from F4 to H4
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000E0000000ul);

        // bit mask 0x000000000F000000: all bits horizontal from D4 to A4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x000000000F000000ul);

        return flipped;
    }


    public static ulong Flip_F4(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x2020202000000000: all bits vertical from F5 to F8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x2020202000000000ul);

        // bit mask 0x0000000000202020: all bits vertical from F3 to F1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000202020ul);

        // bit mask 0x0204081000000000: all bits diagonal from E5 to B8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0204081000000000ul);

        // bit mask 0x0000804000000000: all bits diagonal from G5 to H6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000804000000000ul);

        // bit mask 0x0000000000100804: all bits diagonal from E3 to C1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000100804ul);

        // bit mask 0x0000000000408000: all bits diagonal from G3 to H2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000408000ul);

        // bit mask 0x00000000C0000000: all bits horizontal from G4 to H4
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00000000C0000000ul);

        // bit mask 0x000000001F000000: all bits horizontal from E4 to A4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x000000001F000000ul);

        return flipped;
    }


    public static ulong Flip_G4(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x4040404000000000: all bits vertical from G5 to G8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x4040404000000000ul);

        // bit mask 0x0000000000404040: all bits vertical from G3 to G1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000404040ul);

        // bit mask 0x0408102000000000: all bits diagonal from F5 to C8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0408102000000000ul);

        // bit mask 0x0000000000201008: all bits diagonal from C3 to D1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000201008ul);

        // bit mask 0x000000003F000000: all bits horizontal from F4 to A4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x000000003F000000ul);

        return flipped;
    }


    public static ulong Flip_H4(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x8080808000000000: all bits vertical from H5 to H8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x8080808000000000ul);

        // bit mask 0x0000000000808080: all bits vertical from H3 to H1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000808080ul);

        // bit mask 0x0810204000000000: all bits vertical from G5 to D8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0810204000000000ul);

        // bit mask 0x0000000000402010: all bits vertical from G3 to E1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000000402010ul);

        // bit mask 0x000000007F000000: all bits vertical from G4 to A4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x000000007F000000ul);

        return flipped;
    }


    public static ulong Flip_A5(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0101010000000000: all bits vertical from A6 to A8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0101010000000000ul);

        // bit mask 0x0000000001010101: all bits vertical from A4 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000001010101ul);

        // bit mask 0x0804020000000000: fiels diagonal from B6 to D8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0804020000000000ul);

        // bis mask 0x0000000002040810: all bits diagonal from B4 to E1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000002040810ul);

        // bit mask 0x000000FE00000000: all bits horizontal from B5 to H5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000FE00000000ul);

        return flipped;
    }


    public static ulong Flip_B5(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0202020000000000: all bits vertical from B6 to B8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0202020000000000ul);

        // bit mask 0x0000000002020202: all bits vertical from B4 to B1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000002020202ul);

        // bit mask 0x1008040000000000: all bits diagonal from C6 to E8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x1008040000000000ul);

        // bit mask 0x0000000004081020: all bits diagonal from C4 to F1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000004081020ul);

        // bit mask 0x000000FC00000000: all bits diagonal from C5 to H5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000FC00000000ul);

        return flipped;
    }


    public static ulong Flip_C5(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0404040000000000: all bits vertical from C6 to C8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0404040000000000ul);

        // bit mask 0x0000000004040404: all bits vertical from C4 to C1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000004040404ul);

        // bit mask 0x0001020000000000: all bits diagonal from B6 to A7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0001020000000000ul);

        // bit mask 0x2010080000000000: all bits diagonal from D6 to F8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x2010080000000000ul);

        // bit mask 0x0000000002010000: all bits diagonal from B4 to A3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000002010000ul);

        // bit mask 0x0000000008102040: all bits diagonal from D4 to G1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000008102040ul);

        // bit mask 0x000000F800000000: all bits horizontal from D5 to H5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000F800000000ul);

        // bit mask 0x0000000300000000: all bits horizontal from B5 to A5
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000300000000ul);

        return flipped;
    }


    public static ulong Flip_D5(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0808080000000000: all bits vertical from D6 to D8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0808080000000000ul);

        // bit mask 0x0000000008080808: all bits vertical from D4 to D1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000008080808ul);

        // bit mask 0x0102040000000000: all bits diagonal from C6 to A8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0102040000000000ul);

        // bit mask 0x4020100000000000: all bits diagonal from E6 to G8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x4020100000000000ul);

        // bit mask 0x0000000004020100: all bits diagonal from C4 to A2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000004020100ul);

        // bit mask 0x0000000010204080: all bits diagonal from E4 to H1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000010204080ul);

        // bit mask 0x000000F000000000: all bits horizontal from E5 to H5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000F000000000ul);

        // bit mask 0x0000000700000000: all bits horizontal from C5 to A5
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000700000000ul);

        return flipped;
    }


    public static ulong Flip_E5(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x1010100000000000: all bits vertical from E6 to E8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x1010100000000000ul);

        // bit mask 0x0000000010101010: all bits vertical from E4 to E1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000010101010ul);

        // bit mask 0x0204080000000000: all bits diagonal from D6 to B8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0204080000000000ul);

        // bit mask 0x8040200000000000: all bits diagonal from F6 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x8040200000000000ul);

        // bit mask 0x0000000008040201: all bits diagonal from D4 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000008040201ul);

        // bit mask 0x0000000020408000: all bits diagonal from F4 to H2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000020408000ul);

        // bit mask 0x000000E000000000: all bits horizontal from F5 to H5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000E000000000ul);

        // bit mask 0x0000000F00000000: all bits horizontal from D5 to A5
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000F00000000ul);

        return flipped;
    }


    public static ulong Flip_F5(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x2020200000000000: all bits vertical from F6 to F8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x2020200000000000ul);

        // bit mask 0x0000000020202020: all bits vertical from F4 to F1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000020202020ul);

        // bit mask 0x0408100000000000: all bits diagonal from E6 to C8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0408100000000000ul);

        // bit mask 0x0080400000000000: all bits diagonal from G6 to H7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0080400000000000ul);

        // bit mask 0x0000000010080402: all bits diagonal from E4 to B1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000010080402ul);

        // bit mask 0x0000000040800000: all bits diagonal from G4 to H3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000040800000ul);

        // bit mask 0x000000C000000000: all bits horizontal from G5 to H5
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x000000C000000000ul);

        // bit mask 0x0000001F00000000: all bits horizontal from E5 to A5
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000001F00000000ul);

        return flipped;
    }


    public static ulong Flip_G5(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x4040400000000000: all bits vertical from G6 to G8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x4040400000000000ul);

        // bit mask 0x0000000040404040: all bits vertical from G4 to G1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000040404040ul);

        // bit mask 0x0810200000000000: all bits diagonal from F6 to D8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0810200000000000ul);

        // bit mask 0x0000000020100804: all bits diagonal from F4 to C1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000020100804ul);

        // bit mask 0x0000003F00000000: all bits horizontal from F5 to A5
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000003F00000000ul);

        return flipped;
    }


    public static ulong Flip_H5(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x8080800000000000: all bits vertical from H6 to H8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x8080800000000000ul);

        // bit mask 0x0000000080808080: all bits vertical from H4 to H1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000080808080ul);

        // bit mask 0x1020400000000000: all bits diagonal from G6 to E8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x1020400000000000ul);

        // bit mask 0x0000000040201008: all bits diagonal from G4 to D1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000040201008ul);

        // bit mask 0x0000007F00000000: all bits vertical from G5 to A5
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000007F00000000ul);

        return flipped;
    }


    public static ulong Flip_A6(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0101000000000000: all bits vertical from A7 to A8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0101000000000000ul);

        // bit mask 0x0000000101010101: all bits vertical from A5 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000101010101ul);

        // bit mask 0x0402000000000000: fiels diagonal from B7 to C8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0402000000000000ul);

        // bis mask 0x0000000204081020: all bits diagonal from B5 to F1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000204081020ul);

        // bit mask 0x0000FE0000000000: all bits horizontal from B6 to H6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000FE0000000000ul);

        return flipped;
    }


    public static ulong Flip_B6(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0202000000000000: all bits vertical from B7 to B8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0202000000000000ul);

        // bit mask 0x0000000202020202: all bits vertical from B5 to B1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000202020202ul);

        // bit mask 0x0804000000000000: all bits diagonal from C7 to D8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0804000000000000ul);

        // bit mask 0x0000000408102040: all bits diagonal from C5 to G1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000408102040ul);

        // bit mask 0x0000FC0000000000: all bits diagonal from C6 to H6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000FC0000000000ul);

        return flipped;
    }


    public static ulong Flip_C6(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0404000000000000: all bits vertical from C7 to C8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0404000000000000ul);

        // bit mask 0x0000000404040404: all bits vertical from C5 to C1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000404040404ul);

        // bit mask 0x0102000000000000: all bits diagonal from B7 to A8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0102000000000000ul);

        // bit mask 0x1008000000000000: all bits diagonal from D7 to E8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x1008000000000000ul);

        // bit mask 0x0000000201000000: all bits diagonal from B5 to A4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000201000000ul);

        // bit mask 0x0000000810204080: all bits diagonal from D5 to H1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000810204080ul);

        // bit mask 0x0000F80000000000: all bits horizontal from D6 to H6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000F80000000000ul);

        // bit mask 0x0000030000000000: all bits horizontal from B6 to A6
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000030000000000ul);

        return flipped;
    }


    public static ulong Flip_D6(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0808000000000000: all bits vertical from D7 to D8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x0808000000000000ul);

        // bit mask 0x0000000808080808: all bits vertical from D5 to D1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000808080808ul);

        // bit mask 0x0204000000000000: all bits diagonal from C7 to B8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0204000000000000ul);

        // bit mask 0x2010000000000000: all bits diagonal from E7 to F8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x2010000000000000ul);

        // bit mask 0x0000000402010000: all bits diagonal from C5 to A3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000402010000ul);

        // bit mask 0x0000001020408000: all bits diagonal from E5 to H2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000001020408000ul);

        // bit mask 0x0000F00000000000: all bits horizontal from E6 to H6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000F00000000000ul);

        // bit mask 0x0000070000000000: all bits horizontal from C6 to A6
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000070000000000ul);

        return flipped;
    }


    public static ulong Flip_E6(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x1010000000000000: all bits vertical from E7 to E8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x1010000000000000ul);

        // bit mask 0x0000001010101010: all bits vertical from E5 to E1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000001010101010ul);

        // bit mask 0x0408000000000000: all bits diagonal from D7 to C8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0408000000000000ul);

        // bit mask 0x4020000000000000: all bits diagonal from F7 to G8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x4020000000000000ul);

        // bit mask 0x0000000804020100: all bits diagonal from D5 to A2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000000804020100ul);

        // bit mask 0x0000002040800000: all bits diagonal from F6 to H3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000002040800000ul);

        // bit mask 0x0000E00000000000: all bits horizontal from F6 to H6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000E00000000000ul);

        // bit mask 0x00000F0000000000: all bits horizontal from D6 to A6
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x00000F0000000000ul);

        return flipped;
    }


    public static ulong Flip_F6(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x2020000000000000: all bits vertical from F7 to F8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x2020000000000000ul);

        // bit mask 0x0000002020202020: all bits vertical from F5 to F1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000002020202020ul);

        // bit mask 0x0810000000000000: all bits diagonal from E7 to D8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0810000000000000ul);

        // bit mask 0x8040000000000000: all bits diagonal from G7 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x8040000000000000ul);

        // bit mask 0x0000001008040201: all bits diagonal from E5 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000001008040201ul);

        // bit mask 0x0000004080000000: all bits diagonal from G5 to H4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000004080000000ul);

        // bit mask 0x0000C00000000000: all bits horizontal from G6 to H6
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x0000C00000000000ul);

        // bit mask 0x00001F0000000000: all bits horizontal from E6 to A6
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x00001F0000000000ul);

        return flipped;
    }


    public static ulong Flip_G6(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x4040000000000000: all bits vertical from G7 to G8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x4040000000000000ul);

        // bit mask 0x0000004040404040: all bits vertical from G5 to G1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000004040404040ul);

        // bit mask 0x1020000000000000: all bits diagonal from F7 to E8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x1020000000000000ul);

        // bit mask 0x0000002010080402: all bits diagonal from F5 to B1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000002010080402ul);

        // bit mask 0x00003F0000000000: all bits horizontal from F6 to A6
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x00003F0000000000ul);

        return flipped;
    }


    public static ulong Flip_H6(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x8080000000000000: all bits vertical from H7 to H8
        flipped = FlipHelper.FlipBitsLeft(player, opponent, 0x8080000000000000ul);

        // bit mask 0x0000008080808080: all bits vertical from H5 to H1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000008080808080ul);

        // bit mask 0x2040000000000000: all bits diagonal from G7 to F8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x2040000000000000ul);

        // bit mask 0x0000004020100804: all bits diagonal from G5 to C1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000004020100804ul);

        // bit mask 0x00007F0000000000: all bits vertical from G6 to A6
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x00007F0000000000ul);

        return flipped;
    }


    public static ulong Flip_A7(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0000010101010101: all bits vertical from A6 to A1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0000010101010101ul);

        // bit mask 0x0000020408102040: all bits diagonal from B6 to G1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000020408102040ul);

        // bit mask 0x00FE000000000000: all bits horizontal from B7 to H7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00FE000000000000ul);

        return flipped;
    }


    public static ulong Flip_B7(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0000020202020202: all bits vertical from B6 to B1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0000020202020202ul);

        // bit mask 0x0000040810204080: all bits diagonal from C6 to H8
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000040810204080ul);

        // bit mask 0x00FC000000000000: all bits diagonal from C7 to H7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00FC000000000000ul);

        return flipped;
    }


    public static ulong Flip_C7(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0000040404040404: all bits vertical from C6 to C1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0000040404040404ul);

        // bit mask 0x0000020100000000: all bits diagonal from B6 to A5
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000020100000000ul);

        // bit mask 0x0000081020408000: all bits diagonal from C7 to A2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000081020408000ul);

        // bit mask 0x00F8000000000000: all bits horizontal from D7 to H7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00F8000000000000ul);

        // bit mask 0x0003000000000000: all bits horizontal from B7 to A7
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0003000000000000ul);

        return flipped;
    }


    public static ulong Flip_D7(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0000080808080808: all bits vertical from D6 to D1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0000080808080808ul);

        // bit mask 0x0000040201000000: all bits diagonal from C6 to A4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000040201000000ul);

        // bit mask 0x0000102040800000: all bits diagonal from E6 to H3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000102040800000ul);

        // bit mask 0x00F0000000000000: all bits horizontal from E7 to H7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00F0000000000000ul);

        // bit mask 0x0007000000000000: all bits horizontal from C7 to A7
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0007000000000000ul);

        return flipped;
    }


    public static ulong Flip_E7(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0000101010101010: all bits vertical from E6 to E1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0000101010101010ul);

        // bit mask 0x00E0000000000000: all bits horizontal from F7 to H7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00E0000000000000ul);

        // bit mask 0x0000080402010000: all bits diagonal from D6 to A3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000080402010000ul);

        // bit mask 0x0000204080000000: all bits diagonal from F6 to H4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000204080000000ul);

        // bit mask 0x000F000000000000: all bits horizontal from D7 to A7
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x000F000000000000ul);

        return flipped;
    }


    public static ulong Flip_F7(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0000202020202020: all bits vertical from F6 to F1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0000202020202020ul);

        // bit mask 0x0000100804020100: all bits diagonal from E6 to A2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000100804020100ul);

        // bit mask 0x0000408000000000: all bits diagonal from G6 to H5
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000408000000000ul);

        // bit mask 0x00C0000000000000: all bits horizontal from G7 to H7
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0x00C0000000000000ul);

        // bit mask 0x001F000000000000: all bits horizontal from E7 to A7
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x001F000000000000ul);

        return flipped;
    }


    public static ulong Flip_G7(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0000404040404040: all bits vertical from G6 to G1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0000404040404040ul);

        // bit mask 0x0000201008040201: all bits diagonal from F6 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000201008040201ul);

        // bit mask 0x003F000000000000: all bits horizontal from F7 to A7
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x003F000000000000ul);

        return flipped;
    }


    public static ulong Flip_H7(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0000808080808080: all bits vertical from H6 to H1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0000808080808080ul);

        // bit mask 0x0000402010080402: all bits diagonal from G6 to B1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0000402010080402ul);

        // bit mask 0x007F000000000000: all bits vertical from G7 to A7
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x007F000000000000ul);

        return flipped;
    }


    public static ulong Flip_A8(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0001010101010101: all bits vertical from A7 to A1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0001010101010101ul);

        // bit mask 0x0002040810204080: all bits diagonal from B7 to H1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0002040810204080ul);

        // bit mask 0xFE00000000000000: all bits horizontal from B8 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0xFE00000000000000ul);

        return flipped;
    }


    public static ulong Flip_B8(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0002020202020202: all bits vertical from B7 to B1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0002020202020202ul);

        // bit mask 0x0004081020408000: all bits diagonal from C7 to H2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0004081020408000ul);

        // bit mask 0xFC00000000000000: all bits diagonal from C8 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0xFC00000000000000ul);

        return flipped;
    }


    public static ulong Flip_C8(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0004040404040404: all bits vertical from C7 to C1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0004040404040404ul);

        // bit mask 0x0002010000000000: all bits diagonal from B7 to A6
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0002010000000000ul);

        // bit mask 0x0008102040800000: all bits diagonal from D7 to H3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0008102040800000ul);

        // bit mask 0xF800000000000000: all bits horizontal from D8 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0xF800000000000000ul);

        // bit mask 0x0300000000000000: all bits horizontal from B8 to A8
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0300000000000000ul);

        return flipped;
    }


    public static ulong Flip_D8(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0008080808080808: all bits vertical from D7 to D1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0008080808080808ul);

        // bit mask 0x0004020100000000: all bits diagonal from C7 to A5
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0004020100000000ul);

        // bit mask 0x0010204080000000: all bits diagonal from E2 to H4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0010204080000000ul);

        // bit mask 0xF000000000000000: all bits horizontal from E8 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0xF000000000000000ul);

        // bit mask 0x0700000000000000: all bits horizontal from C8 to A8
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0700000000000000ul);

        return flipped;
    }


    public static ulong Flip_E8(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0010101010101010: all bits vertical from E7 to E1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0010101010101010ul);

        // bit mask 0x0008040201000000: all bits diagonal from D7 to A4
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0008040201000000ul);

        // bit mask 0x0020408000000000: all bits diagonal from F7 to H5
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0020408000000000ul);

        // bit mask 0xE000000000000000: all bits horizontal from F8 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0xE000000000000000ul);

        // bit mask 0x0F00000000000000: all bits horizontal from D8 to A8
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0F00000000000000ul);

        return flipped;
    }


    public static ulong Flip_F8(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0020202020202020: all bits vertical from F7 to F1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0020202020202020ul);

        // bit mask 0x0010080402010000: all bits diagonal from E7 to A3
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0010080402010000ul);

        // bit mask 0x0040800000000000: all bits diagonal from G7 to H6
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0040800000000000ul);

        // bit mask 0xC000000000000000: all bits horizontal from G8 to H8
        flipped |= FlipHelper.FlipBitsLeft(player, opponent, 0xC000000000000000ul);

        // bit mask 0x1F00000000000000: all bits horizontal from E8 to A8
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x1F00000000000000ul);

        return flipped;
    }


    public static ulong Flip_G8(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0040404040404040: all bits vertical from G7 to G1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0040404040404040ul);

        // bit mask 0x0020100804020100: all bits diagonal from F7 to A2
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0020100804020100ul);

        // bit mask 0x3F00000000000000: all bits horizontal from F8 to A8
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x3F00000000000000ul);

        return flipped;
    }


    public static ulong Flip_H8(ulong player, ulong opponent)
    {
        ulong flipped;

        // bit mask 0x0080808080808080: all bits vertical from H7 to H1
        flipped = FlipHelper.FlipBitsRight(player, opponent, 0x0080808080808080ul);

        // bit mask 0x0040201008040201: all bits diagonal from G7 to A1
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x0040201008040201ul);

        // bit mask 0x7F00000000000000: all bits vertical from G8 to A8
        flipped |= FlipHelper.FlipBitsRight(player, opponent, 0x7F00000000000000ul);

        return flipped;
    }
}