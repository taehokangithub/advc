#include <unordered_set>

#include "gtest/gtest.h"
#include "../../utils/point.h"

using namespace std;

namespace advc_2023::google_test
{
    TEST(Point_tests, add_subs)
    {
        const utils::Point p1(1, 2, 3, 4);
        const utils::Point p2(2, 3, 4, 5);
        const utils::Point expected_add(3, 5, 7, 9);
        const utils::Point expected_subs(-2, -3, -4, -5);

        const auto result_add = p1.get_added(p2);
        EXPECT_EQ(result_add, expected_add);

        const auto result_subs = p1.get_subbed(result_add);
        EXPECT_EQ(result_subs, expected_subs);
    }

    TEST(Point_tests, dim_3)
    {
        const utils::Point p1(-1, 3, 2);
        const utils::Point p2(5, 7, -3);
        const utils::Point expected_add(4, 10, -1);

        const auto result_add = p1.get_added(p2);
        EXPECT_EQ(result_add, expected_add);
    }

    TEST(Point_tests, mul_div)
    {
        const utils::Point p(2, 4, 6, 7);
        const utils::Point expected_mul(6, 12, 18, 21);
        const utils::Point expected_div(1, 2, 3, 3);

        const auto result_mul = p.get_multipled(3);
        EXPECT_EQ(result_mul, expected_mul);

        const auto result_div = p.get_divided(2);
        EXPECT_EQ(result_div, expected_div);
    }

    TEST(Point_tests, move)
    {
        const utils::Point p(10, 11);

        const utils::Point p_up(10, 10);
        const utils::Point p_down(10, 12);
        const utils::Point p_left(9, 11);
        const utils::Point p_right(11, 11);


        const auto result_move_up = p.get_moved(utils::Dir::Up);
        const auto result_move_down = p.get_moved(utils::Dir::Down);
        const auto result_move_left = p.get_moved(utils::Dir::Left);
        const auto result_move_right = p.get_moved(utils::Dir::Right);

        EXPECT_EQ(result_move_up, p_up);
        EXPECT_EQ(result_move_down, p_down);
        EXPECT_EQ(result_move_left, p_left);
        EXPECT_EQ(result_move_right, p_right);
    }

    TEST(Point_tests, rotate)
    {
        const utils::Point p(3, -5);
        const utils::Point p_left(-5, -3);
        const utils::Point p_right(5, 3);

        const auto result_left = p.get_rotated(utils::Dir::Left);
        const auto result_right = p.get_rotated(utils::Dir::Right);

        EXPECT_EQ(result_left, p_left);
        EXPECT_EQ(result_right, p_right);
    }

    TEST(Point_tests, rotate_dir)
    {
        utils::Dir dir{ utils::Dir::Right };

        auto dir1 = utils::Point::get_rotated_dir(dir, utils::Dir::Right);
        auto dir2 = utils::Point::get_rotated_dir(dir1, utils::Dir::Right);
        auto dir3 = utils::Point::get_rotated_dir(dir2, utils::Dir::Right);
        auto dir4 = utils::Point::get_rotated_dir(dir3, utils::Dir::Right);

        EXPECT_NE(dir, dir1);
        EXPECT_NE(dir, dir2);
        EXPECT_NE(dir, dir3);
        EXPECT_EQ(dir, dir4);

        dir = utils::Dir::Down;

        dir1 = utils::Point::get_rotated_dir(dir, utils::Dir::Left);
        dir2 = utils::Point::get_rotated_dir(dir1, utils::Dir::Left);
        dir3 = utils::Point::get_rotated_dir(dir2, utils::Dir::Left);
        dir4 = utils::Point::get_rotated_dir(dir3, utils::Dir::Left);

        EXPECT_NE(dir, dir1);
        EXPECT_NE(dir, dir2);
        EXPECT_NE(dir, dir3);
        EXPECT_EQ(dir, dir4);
    }

    TEST(Point_tests, set_max)
    {
        utils::Point p1(3, -5);
        const utils::Point p1max(2, -2);
        const utils::Point expected_max1(3, -2);

        utils::Point p2(3, -5);
        const utils::Point p2max(5, -7);
        const utils::Point expected_max2(5, -5);

        p1.set_max(p1max);
        p2.set_max(p2max);

        EXPECT_EQ(p1, expected_max1);
        EXPECT_EQ(p2, expected_max2);
    }

    TEST(Point_tests, set_min)
    {
        utils::Point p1(3, -5);
        const utils::Point p1min(2, -2);
        const utils::Point expected1(2, -5);

        utils::Point p2(3, -5);
        const utils::Point p2min(5, -7);
        const utils::Point expected2(3, -7);

        p1.set_min(p1min);
        p2.set_min(p2min);

        EXPECT_EQ(p1, expected1);
        EXPECT_EQ(p2, expected2);
    }

    TEST(Point_tests, unique_strings)
    {
        utils::Point p1(1, 2, 3, 5);
        utils::Point p2(1, 2, 5, 7);
        utils::Point p3(1, 2, 3, 5);

        unordered_set<string> test_set;
        test_set.insert(p1.to_unique_string());
        test_set.insert(p2.to_unique_string());

        EXPECT_EQ((int)test_set.size(), 2);

        test_set.insert(p3.to_unique_string());
        EXPECT_EQ((int)test_set.size(), 2);

        utils::Point p_reverse(p1.to_unique_string());
        EXPECT_EQ(p_reverse, p1);
    }

    TEST(Point_tests, distance_test)
    {
        utils::Point p1(1, 2, 3, -5);
        utils::Point p2(1, 2, -5, 7);
        utils::Point p3(-5, -3, 3, 5);

        EXPECT_EQ(p1.get_manhattan_distance(p2), 20);
        EXPECT_EQ(p1.get_manhattan_distance(p3), 21);
        EXPECT_EQ(p2.get_manhattan_distance(p3), 21);
    }

    TEST(Point_tests, dir_test)
    {
        for (utils::Dir dir : utils::Point::s_dirs)
        {
            const auto opp_dir = utils::Point::get_opposite_dir(dir);
            EXPECT_NE(dir, opp_dir);

            const auto dir_back = utils::Point::get_opposite_dir(opp_dir);
            EXPECT_EQ(dir, dir_back);
        }
    }
}
