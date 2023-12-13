#pragma once

#include <string>
#include <vector>

namespace advc_2023::utils
{
    enum class line_option
    {
        skip_empty_line,
        dont_skip_empty_line
    };

    std::vector<std::string> get_lines(const std::string& file_name, line_option lopt = line_option::skip_empty_line);

    std::vector<std::string> split(const std::string& str, const std::string& delimeter);
}