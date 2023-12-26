#include <unordered_map>
#include <memory>
#include <string>
#include <vector>

namespace advc_2023::day25
{
    struct Edge;

    struct Node
    {
        std::string name;
        std::vector<std::shared_ptr<Edge>> edges;
        int index;
    };

    // --------------------------------------------------------

    struct Edge
    {
        std::shared_ptr<Node> node1, node2;
        std::shared_ptr<Node> get_other(const std::shared_ptr<const Node>& node) { return node == node1 ? node2 : node1; }
        std::string to_string() const;
    };

    // --------------------------------------------------------

    class Components
    {
    public:
        void parse(const std::vector<std::string>& lines);
        int get_clusters_signature() const;

    private:
        std::shared_ptr<Node> get_or_create_node(const std::string& name);
        std::shared_ptr<const Edge> get_most_visited_edge(const std::vector<std::shared_ptr<const Edge>>& disabled) const;
        int get_cluster_count(std::shared_ptr<const Node> start_node, const std::vector<std::shared_ptr<const Edge>>& disabled) const;

        std::unordered_map<std::string, std::shared_ptr<Node>> m_nodes;
        int node_index{};
    };
}