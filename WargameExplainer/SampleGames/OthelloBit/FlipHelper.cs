using System.Numerics;

namespace WargameExplainer.SampleGames.OthelloBit;

public static class FlipHelper
{
    /// <summary>
    ///     subtract ulong by -1, if ulong is non zero
    /// </summary>
    /// <param name="outflank"></param>
    /// <returns></returns>
    public static ulong OutFlankToBitMask(ulong outflank)
    {
        return outflank - (ulong)((outflank != 0) ? 1 : 0);
    }


    /// <summary>
    ///     find the first position in a bit mask, that isn't occupied by the opponent (right flip)
    /// </summary>
    /// <param name="opponent">coding of opponent disks on the board</param>
    /// <param name="maskr">bit pattern of the flip (up, left, diagonal-up in board view, right in bit view)</param>
    /// <returns>return ulong with bit set at first non occupied position in mask</returns>
    public static ulong OutflankRight(ulong opponent, ulong maskr)
    {
        // delete last bit from mask
        // explanation: the last bit of the mask needs to be set even though it can't be flipped (because it is the one at the board edge)
        // for further ongoing a modified mask is created with deleted last bit
        ulong modifiedMask = maskr & (maskr - 1);

        // find positions in mask, that are not occupied by opponent disks
        // last bit of maskr will alwalys be set in bitwiseResult
        // explanation: otherwise there could be the case, that non of the bits would be set
        ulong bitwiseResult = (opponent & modifiedMask) ^ maskr;

        // if no bit would be set, the operation would be 0x8000000000000000 >> (64 mod 64), so there wouldn`t be any change => false behaviour!
        return 0x8000000000000000UL >> BitOperations.LeadingZeroCount(bitwiseResult);
    }


    /// <summary>
    ///     flip left bits (board view: down, right, diagonal-down)
    /// </summary>
    /// <param name="player">coding of player disks on the board</param>
    /// <param name="opponent">coding of opponent disks on the board</param>
    /// <param name="bitPattern">bit pattern of the flip (down, right, diagonal-down in board view, left in bit view)</param>
    /// <returns>bits to be flipped left</returns>
    public static ulong FlipBitsLeft(ulong player, ulong opponent, ulong bitPattern)
    {
        // find enclosing player disk with bit mask
        ulong outflank_down = ((opponent | ~bitPattern) + 1) & player & bitPattern;

        return OutFlankToBitMask(outflank_down) & bitPattern;
    }


    /// <summary>
    ///     flip right bits (board view: up, left, diagonal-up)
    /// </summary>
    /// <param name="player">coding of player disks on the board</param>
    /// <param name="opponent">coding of opponent disks on the board</param>
    /// <param name="bitPattern">bit pattern of the flip (up, left, diagonal-up in board view, right in bit view)</param>
    /// <returns>bits to be flipped right</returns>
    public static ulong FlipBitsRight(ulong player, ulong opponent, ulong bitPattern)
    {
        // find first enclosing player disk
        ulong outflank_up = OutflankRight(opponent, bitPattern) & player;

        // set all bits lower than player disk position (inclusive)
        // revert all bits, so that all higher bits are set and mask with bit pattern
        ulong flipped = ~((outflank_up << 1) - 1) & bitPattern;
        return flipped;
    }
}