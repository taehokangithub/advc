
#include <assert.h>

#include "../../utils/input_utils.h"
#include "games.h"

namespace advc_2023::day02
{
    // ----------------------------------------

    void Draw::parse(const string& contents)
    {
        static const string s_blue("blue");
        static const string s_red("red");
        static const string s_green("green");

        const auto& contents_strings = utils::split(contents, ", ");

        for (const auto& content : contents_strings)
        {
            const auto& content_desc = utils::split(content, " ");
            assert(content_desc.size() == 2);

            Element element;
            element.value = stoi(content_desc.front());
            assert(element.value > 0);

            const auto& colour_str = content_desc.back();

            element.colour = (colour_str == s_blue  ? Colour::blue :
                             (colour_str == s_green ? Colour::green :
                             (colour_str == s_red   ? Colour::red : 
                                                      Colour::max)));

            assert(element.colour != Colour::max);
            m_elements.push_back(element);
        }
    }

    // ----------------------------------------

    bool Draw::is_passed(const Filter_type& filter) const
    {
        for (const auto& element : m_elements)
        {
            if (const auto& ite = filter.find(element.colour); ite != filter.end())
            {
                if (ite->second < element.value)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // ----------------------------------------

    void Game::parse(const string& line)
    {
        const auto& split_lines = utils::split(line, ": ");

        assert(split_lines.size() == 2);

        const auto& game_title = split_lines.front();

        const auto& split_game_title = utils::split(game_title, " ");
        assert(split_game_title.size() == 2);
        assert(split_game_title.front() == "Game");

        const auto& game_id_str = split_game_title.back();
        m_id = stoi(game_id_str);

        const auto& game_content = split_lines.back();
        const auto& draw_strings = utils::split(game_content, "; ");

        for (const auto& draw_string : draw_strings)
        {
            auto draw = make_shared<Draw>();
            draw->parse(draw_string);
            m_draws.push_back(draw);
        }
    }

    // ----------------------------------------

    bool Game::is_passed(const Filter_type& filter) const
    {
        for (const auto& draw : m_draws)
        {
            if (!draw->is_passed(filter))
            {
                return false;
            }
        }
        return true;
    }

    // ----------------------------------------

    int Game::get_power() const
    {
        Filter_type max_requirements;

        for (const auto& draw : m_draws)
        {
            for (const auto& element : draw->get_elements())
            {
                max_requirements[element.colour] = max(element.value, max_requirements[element.colour]);
            }
        }

        int sum = 1;
        for (const auto& [colour, value] : max_requirements)
        {
            sum *= value;
        }

        return sum;
    }

    // ----------------------------------------

    void Games::parse(const vector<string>& lines)
    {
        for (const string& line : lines)
        {
            auto game = make_shared<Game>();
            game->parse(line);
            m_games.push_back(game);
        }
    }

    // ----------------------------------------

    int Games::get_sum_of_passed_games(const Filter_type& filter) const
    {
        int sum = 0;
        for (const auto& game : m_games)
        {
            if (game->is_passed(filter))
            {
                sum += game->get_id();
            }
        }
        return sum;
    }

    // ----------------------------------------
    
    int Games::get_sum_of_powers() const
    {
        int sum = 0;
        for (const auto& game : m_games)
        {
            sum += game->get_power();
        }
        return sum;
    }
}