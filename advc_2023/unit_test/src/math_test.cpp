
#include "gtest/gtest.h"
#include "../../utils/grid.h"
#include "../../utils/math.h"

using namespace std;

namespace advc_2023::google_test
{
    TEST(Gaussian_elimination_test, math_test)
    {
        const auto validate_answer = [](const vector<vector<double>>& my_vector, const vector<double>& answer)
            {
                for (int i = 0; i < (int)my_vector.size(); i++)
                {
                    const auto& row = my_vector[i];
                    auto expected = row[0] * answer[0] + row[1] * answer[1] + row[2] * answer[2] + row[3] * answer[3];
                    EXPECT_EQ(utils::is_almost_same(expected, row[4]), true);
                }
            };

        const vector<vector<double>> my_vector1{
            { 3, 4, 8, 9, 11 },
            { 2, 7, 1, -2, 13 },
            { 3, 13, -9, -8, 3 },
            { -5, -6, 4, 15, -1 }
        };

        auto ans = utils::gaussian_elimination(my_vector1);
        EXPECT_EQ(ans.size(), 4);
        validate_answer(my_vector1, ans);


        const vector<vector<double>> my_vector2{
            { 0, 15, 33, 21, -7 },
            { 1, 0, 42, -16, -6 },
            { 42, -5, 0, 77, 100 },
            { -16, -7, -3, 0, 20 }
        };

        ans = utils::gaussian_elimination(my_vector2);
        EXPECT_EQ(ans.size(), 4);
        validate_answer(my_vector2, ans);

        const vector<vector<double>> my_vector3{
            { 0, 1, 2, 3, -7 },
            { 0, 2, 4, 6, -6 },
            { 1, 3, 5, 7, 100 },
            { 2, 4, 6, 8, 20 }
        };

        ans = utils::gaussian_elimination(my_vector3);
        EXPECT_EQ(ans.size(), 0);

    }

}
