#pragma once

#include <string>
#include <vector>

namespace advc_2023::utils
{
    std::vector<std::string> get_lines(const std::string& file_name);

    std::vector<std::string> split(const std::string& str, const std::string& delimeter);
}