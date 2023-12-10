#include <unordered_set>

#include "gtest/gtest.h"
#include "../../utils/map.h"

using namespace std;

namespace advc_2023::google_test
{
    TEST(Map_tests, basic_tests_2d)
    {
        utils::Map<int> my_map;

        my_map.set(utils::Point(1, 5), 3);
        my_map.set(utils::Point(-3, 15), 2);
        my_map.set(utils::Point(7, -22), 5);
        my_map.set(utils::Point(-6, 18), 3);
        my_map.set(utils::Point(-15, 29), 7);
        my_map.set(utils::Point(10, -11), 9);
        my_map.set(utils::Point(-6, 18), 100);

        utils::Point expected_min(-15, -22);
        utils::Point expected_max(10, 29);

        EXPECT_EQ(expected_min, my_map.get_min());
        EXPECT_EQ(expected_max, my_map.get_max());
        EXPECT_EQ(my_map.exists(utils::Point(-6, 18)), true);
        EXPECT_EQ(my_map.exists(utils::Point(-6, 19)), false);
        EXPECT_EQ(my_map.get(-3,15), 2);
        EXPECT_EQ(my_map.get(-6, 18), 100);
        EXPECT_EQ(my_map.is_valid_point(utils::Point(-15, 29)), true);
        EXPECT_EQ(my_map.is_valid_point(utils::Point(-16, 3)), false);
        EXPECT_EQ(my_map.is_valid_point(utils::Point(-15, 30)), false);

        const auto& data = my_map.get_data();

        EXPECT_EQ((int)data.size(), 6);
    }

    TEST(Map_tests, basic_tests_3d)
    {
        utils::Map<bool> my_map;

        my_map.set(utils::Point(1, 5, 3), true);
        my_map.set(utils::Point(-3, 15, 17), true);
        my_map.set(utils::Point(7, -22, 20), true);
        my_map.set(utils::Point(-6, 18, -1), true);
        my_map.set(utils::Point(-15, 29, -3), true);
        my_map.set(utils::Point(10, -11, 5), false);
        my_map.set(utils::Point(-6, 18, -1), false);

        utils::Point expected_min(-15, -22, -3);
        utils::Point expected_max(10, 29, 20);

        EXPECT_EQ(expected_min, my_map.get_min());
        EXPECT_EQ(expected_max, my_map.get_max());

        EXPECT_EQ(my_map.exists(utils::Point(-6, 18, -1)), true);
        EXPECT_EQ(my_map.exists(utils::Point(-6, 18, -2)), false);
        EXPECT_EQ(my_map.get(-3, 15, 17), true);
        EXPECT_EQ(my_map.get(-6, 18, -1), false);
        EXPECT_EQ(my_map.is_valid_point(utils::Point(-15, 29, 20)), true);
        EXPECT_EQ(my_map.is_valid_point(utils::Point(-15, 20, 21)), false);
        EXPECT_EQ(my_map.is_valid_point(utils::Point(10, -22, -3)), true);
        EXPECT_EQ(my_map.is_valid_point(utils::Point(10, -22, -4)), false);

        const auto& data = my_map.get_data();
        EXPECT_EQ((int)data.size(), 6);
    }

    TEST(Map_tests, basic_tests_4d)
    {
        utils::Map<bool> my_map;

        my_map.set(utils::Point(1, 5, 3, -1), true);
        my_map.set(utils::Point(-3, 15, 17, -2), true);
        my_map.set(utils::Point(7, -22, 20, -15), true);
        my_map.set(utils::Point(-6, 18, -1, 1), true);
        my_map.set(utils::Point(-15, 29, -3, 5), true);
        my_map.set(utils::Point(10, -11, 5, 25), false);
        my_map.set(utils::Point(-6, 18, -1, 1), false);

        utils::Point expected_min(-15, -22, -3, -15);
        utils::Point expected_max(10, 29, 20, 25);

        EXPECT_EQ(expected_min, my_map.get_min());
        EXPECT_EQ(expected_max, my_map.get_max());

        EXPECT_EQ(my_map.exists(utils::Point(-6, 18, -1, 1)), true);
        EXPECT_EQ(my_map.exists(utils::Point(-6, 18, -1, 2)), false);
        EXPECT_EQ(my_map.get(utils::Point(-3, 15, 17, -2)), true);
        EXPECT_EQ(my_map.get(utils::Point(-6, 18, -1, 1)), false);
        EXPECT_EQ(my_map.is_valid_point(utils::Point(-15, 29, 20, 25)), true);
        EXPECT_EQ(my_map.is_valid_point(utils::Point(-15, 20, 19, 26)), false);
        EXPECT_EQ(my_map.is_valid_point(utils::Point(10, -22, -3, -15)), true);
        EXPECT_EQ(my_map.is_valid_point(utils::Point(10, -22, -3, -16)), false);

        const auto& data = my_map.get_data();
        EXPECT_EQ((int)data.size(), 6);
    }

    TEST(Map_tests, operator_test)
    {
        utils::Point a(1, 5, 7, 8);
        utils::Point b(1, 5, 7, 9);
        utils::Point c(1, 5, 5, 8);
        utils::Point d(1, 3, 5, 8);
        utils::Point e(1, 6, 5, 8);
        utils::Point f(2, 5, 7, 8);
        utils::Point g(-1, 5, 7, 8);
        utils::Point h(2, -1, -2, -3);

        EXPECT_TRUE(a < b);
        EXPECT_FALSE(b < a);
        EXPECT_TRUE(c < a);
        EXPECT_FALSE(a < c);
        EXPECT_TRUE(d < a);
        EXPECT_FALSE(a < d);
        EXPECT_TRUE(a < e);
        EXPECT_FALSE(e < a);
        EXPECT_TRUE(a < f);
        EXPECT_FALSE(f < a);
        EXPECT_TRUE(g < a);
        EXPECT_FALSE(a < g);
        EXPECT_TRUE(a < h);
        EXPECT_FALSE(h < a);
    }
}
