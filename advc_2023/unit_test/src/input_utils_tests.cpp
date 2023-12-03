#include "gtest/gtest.h"
#include "../../utils/input_utils.h"


using namespace std;

namespace advc_2023::google_test
{
    TEST(Input_utils_tests, Split_test_four)
    {
        const string sample_data = "i => am => a => boy";
        const auto& result = advc_2023::utils::split(sample_data, " => ");

        EXPECT_EQ((int)result.size(), 4);
        EXPECT_EQ(result[0], string("i"));
        EXPECT_EQ(result[1], string("am"));
        EXPECT_EQ(result[2], string("a"));
        EXPECT_EQ(result[3], string("boy"));
    }

    TEST(Input_utils_tests, Split_test_one)
    {
        const string sample_data = "i => am => a => boy";
        const auto& result = advc_2023::utils::split(sample_data, "::");

        EXPECT_EQ((int)result.size(), 1);
        EXPECT_EQ(result[0], sample_data);
    }
}
