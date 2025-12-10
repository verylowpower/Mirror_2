using System.Collections.Generic;
using System.Linq;

public static class ExpTable
{
    private static readonly List<long> expChart = new()
    {
        // exp needed for each level (index = level)
        1, 10, 22, 34, 48, 56, 73, 105, 145, 192,  // 1 - 10
        252, 326, 410, 505, 610, 750, 910, 1150, 1420, 1710, // 11 - 20
        2020, 2350, 2700, 3100, 3530, 3985, 4480, 5030, 5610, 6216, // 21 - 30
        6870, 7550, 8290, 9100, 10000, 11000, 12400, 14000, 15800, 17800, // 31 - 40
        20000, 22500, 25300, 28500, 32000, 35700, 39600, 44000, 48600, 53500, // 41 - 50
    };

    public static long GetExpRequired(int currentLevel)
    {
        if (currentLevel <= 0)
            return 0;

        int index = currentLevel - 1;

        if (index >= expChart.Count)
            return expChart.Last();

        return expChart[index];
    }
}
