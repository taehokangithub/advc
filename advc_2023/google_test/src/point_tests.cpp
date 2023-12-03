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
}
