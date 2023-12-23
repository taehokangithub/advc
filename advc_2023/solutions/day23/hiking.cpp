#include <assert.h>
#include <iostream>
#include <queue>
#include <stack>

#include "../../utils/input_utils.h"
#include "hiking.h"
#include "graph.h"

using namespace std;

namespace advc_2023::day23
{
    // ------------------------------------------------

    static utils::Dir get_dir_from_tile(const tile_type tt)
    {
        switch (tt)
        {
        case tile_type::slope_up: return utils::Dir::Up;
        case tile_type::slope_left: return utils::Dir::Left;
        case tile_type::slope_right: return utils::Dir::Right;
        case tile_type::slope_down: return utils::Dir::Down;
        default:
            assert(false);
        }
        return utils::Dir();
    }

    // ------------------------------------------------

    static tile_type get_tile_from_char(const char c)
    {
        switch (c)
        {
        case '#': return tile_type::forest;
        case '.': return tile_type::path;
        case '^': return tile_type::slope_up;
        case 'v': return tile_type::slope_down;
        case '>': return tile_type::slope_right;
        case '<': return tile_type::slope_left;
        default:
            assert(false);
        }
        return tile_type::none;
    }

    // ------------------------------------------------

    static char get_char_from_tile(const tile_type tt)
    {
        switch (tt)
        {
        case tile_type::slope_up: return '^';
        case tile_type::slope_left: return '<';
        case tile_type::slope_right: return '>';
        case tile_type::slope_down: return 'v';
        case tile_type::forest: return '#';
        case tile_type::path: return '.';
        case tile_type::visited: return '0';
        default:
            assert(false);
        }
        return '?';
    }

    // ------------------------------------------------

    static bool is_tile_slope(const tile_type tt)
    {
        switch (tt)
        {
        case tile_type::slope_up:
        case tile_type::slope_down:
        case tile_type::slope_right:
        case tile_type::slope_left:
            return true;
        }
        return false;
    }

    // ------------------------------------------------

    void Hiking::parse(const vector<string>& lines)
    {
        m_grid.set_size((int)lines.front().size(), (int)lines.size());

        for (const auto& line : lines)
        {
            for (const char c : line)
            {
                m_grid.add_element(get_tile_from_char(c));
            }
        }
        
        m_enter_loc = { 1, 0 };
        m_exit_loc = { (int)m_grid.get_size().x - 2, (int)m_grid.get_size().y - 1 };

        assert(m_grid.is_all_set());
        assert(m_grid.get(m_enter_loc) == tile_type::path);
        assert(m_grid.get(m_exit_loc) == tile_type::path);
    }

    // ------------------------------------------------

    void Search::draw() const
    {
        for (int y = 0; y < (int)m_grid.get_size().y; y++)
        {
            for (int x = 0; x < (int)m_grid.get_size().x; x++)
            {
                cout << get_char_from_tile(m_grid.get(x, y));
            }
            cout << endl;
        }
    }


    // --------------------------------------------------------

    search_list Search::get_possible_steps(std::shared_ptr<Search> search, const search_type st)
    {
        search_list searches;

        const auto curpos = search->m_curpos; // memorise it because it may change
        const auto next_steps = search->m_steps + 1;

        for (const utils::Dir dir : utils::Point::s_dirs)
        {
            const utils::Point next_pos = curpos.get_moved(dir);

            if (!search->m_grid.is_valid_point(next_pos))
            {
                continue;
            }

            const tile_type tt = search->m_grid.get(next_pos);

            bool can_go{ false };
            if (tt == tile_type::path)
            {
                can_go = true;
            }
            else if (is_tile_slope(tt))
            {
                if (st == search_type::ignore_slope)
                {
                    can_go = true;
                }
                else
                {
                    const auto slope_dir = get_dir_from_tile(tt);
                    if (dir == slope_dir)
                    {
                        can_go = true;
                    }
                }
            }

            if (can_go)
            {
                if (searches.empty())
                {
                    searches.push_back(search); //  always try to reuse the object if there's only 1 possibility
                }
                else
                {
                    searches.push_back(make_shared<Search>(*search));
                }

                auto new_search = searches.back();

                new_search->m_curpos = next_pos;
                new_search->m_steps = next_steps;
            }
        }

        return searches;
    }

    // ------------------------------------------------

    int Hiking::get_longest_path() const
    {
        std::queue<shared_ptr<Search>> q;
        auto search = make_shared<Search>(m_grid, m_enter_loc);
        
        q.push(search);
        int longest_path = 0;

        while (!q.empty())
        {
            search = q.front(); q.pop();
            search->set_visited();

            const auto curpos = search->get_curpos();

            const auto next_searches = Search::get_possible_steps(search, search_type::normal);

            //cout << curpos.to_string() << "(" << search->get_steps() << " steps) yields " << next_searches.size() << " next steps. q " << q.size() << endl;

            for (const auto next_search : next_searches)
            {
                if (next_search->get_curpos() == m_exit_loc)
                {
                    //cout << "found exit " << next_search->get_steps() << endl;
                    longest_path = std::max(next_search->get_steps(), longest_path);
                }

                q.push(next_search);
            }
        }

        return longest_path;
    }

    // ------------------------------------------------

    int Hiking::get_longest_path_ignore_slope() const
    {
        Graph g(m_grid, m_enter_loc, m_exit_loc);

        return g.find_longest_path();
    }

}