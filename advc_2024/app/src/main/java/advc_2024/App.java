/*
 * This Java source file was generated by the Gradle 'init' task.
 */
package advc_2024;

public class App {
    public static void main(String[] args) {
        final long startTime = System.currentTimeMillis();

        new advc_2024.day00_template.Solution().run();
        new advc_2024.day01.Solution().run();
        new advc_2024.day02.Solution().run();
        new advc_2024.day03.Solution().run();

        System.out.println();
        System.out.println(String.format("Finished - total %dms", System.currentTimeMillis() - startTime));
    }
}