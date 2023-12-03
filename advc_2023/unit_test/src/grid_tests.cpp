#include "gtest/gtest.h"
#include "../../utils/grid.h"

using namespace std;

namespace advc_2023::google_test
{
    TEST(Grid_tests, basic_grid_test)
    {
        utils::Grid<int> grid;
        const utils::Point size(10, 5);
        const int cnt = size.x * size.y;

        grid.set_size(size);

        for (int i = 0; i < cnt; i++)
        {
            EXPECT_EQ(grid.is_all_set(), false);
            grid.add_element(i);
        }

        EXPECT_EQ(grid.is_all_set(), true);
        EXPECT_EQ(grid.get(utils::Point(3, 2)), 23);
        EXPECT_EQ(grid.get(3, 2), 23);

        const utils::Point to_set(7, 3);
        const int to_set_value = -11;
        grid.set(to_set, to_set_value);
        EXPECT_EQ(grid.get(to_set), to_set_value);
        EXPECT_EQ(grid.get_size(), size);

        EXPECT_EQ(grid.is_valid_point(10, 3), false);
        EXPECT_EQ(grid.is_valid_point(-1, 3), false);
        EXPECT_EQ(grid.is_valid_point(2, 5), false);
        EXPECT_EQ(grid.is_valid_point(3, 6), false);
        EXPECT_EQ(grid.is_valid_point(4, -1), false);
        EXPECT_EQ(grid.is_valid_point(5, 0), true);
        EXPECT_EQ(grid.is_valid_point(0, 3), true);
        EXPECT_EQ(grid.is_valid_point(2, 3), true);
    }

}
