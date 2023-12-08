#pragma once

#include <map>
#include <memory>
#include <string>
#include <vector>


namespace advc_2023::day08
{
    class Node
    {
    public:
        Node(const std::string& name) : m_name(name) {}

        void                        set_links(const std::shared_ptr<Node>& left, const std::shared_ptr<Node>& right);

        const std::string&          get_name() const { return m_name; }
        std::shared_ptr<const Node> get_next(char inst) const;
        bool                        has_postfix(char c) const { return m_name.back() == c; }

    private:
        std::string m_name;
        std::shared_ptr<Node> m_left;
        std::shared_ptr<Node> m_right;
    };

    // ----------------------------------------------------

    class Graph
    {
    public:
        std::shared_ptr<Node> get_or_create_node(const std::string& name);
        std::shared_ptr<Node> get_node(const std::string& name) const;

        std::vector<std::shared_ptr<const Node>> get_all_nodes_end_with(char c) const;

    private:
        std::map<std::string, std::shared_ptr<Node>> m_nodes;
    };

    // ----------------------------------------------------

    class Navigator
    {
    public:
        void        parse(const std::vector<std::string>& lines);
        int         get_steps() const;
        uint64_t    get_multiple_steps() const;

    private:
        std::string m_instruction;
        Graph m_graph;
    };
}