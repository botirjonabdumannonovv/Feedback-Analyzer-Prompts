using Microsoft.SemanticKernel;

class Program
{
    static async Task Main(string[] args)
    {
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(modelId: "gpt-3.5-turbo", apiKey: "YourApiKeyHere")
            .Build();

        string productDescription = "This smartphone features a 6.5-inch retina display, triple-camera system, and 128GB storage.";

        Console.Write("Enter your feedback about the product: ");
        var userFeedback = Console.ReadLine();

        var relevancePrompt = $@"
            ## Feedback Analysis for Product and Service Relevance

            Product Description:
            {productDescription}

            User Feedback:
            {userFeedback}

            ## Evaluation Objective

            Determine the relevance of the user feedback to the product and associated services. Assess whether the feedback pertains to the product's features, performance, user experience, or service quality.

            ## Analysis Criteria

            - Contextual Relevance: Evaluate if the feedback addresses any aspects of the product or associated services mentioned in the product description.
            - Relevance to User Experience and Services: Determine if the feedback relates to the overall usage, experience of using the product, or satisfaction with associated services.
            - Actionability: Assess whether the feedback is specific and actionable, relating to either the product's features or service aspects.

            ## Decision

            Classify the feedback as:
            - Relevant: If it pertains to the product's features, performance, user experience, or associated service quality.
            - Not Relevant: If it does not relate to the product's specific details, intended use, or associated services.

            ## Conclusion

            Based on the criteria above, decide if the user feedback is relevant to the product or its associated services. Provide a boolean value: 'true' for relevant, 'false' for not relevant.
        ";

        // Invoke the Semantic Kernel for relevance analysis
        var response = await kernel.InvokePromptAsync<string>(relevancePrompt);

        // Process the response to handle any formatting issues
        bool isRelevant = response?.Trim().Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;

        // Create the FeedbackRelevancy model
        var feedbackRelevancy = new FeedbackRelevancy { IsRelevant = isRelevant };

        // Output the relevance result
        Console.WriteLine($"Is the feedback relevant to the product or service? {feedbackRelevancy.IsRelevant}");
    }

    public class FeedbackRelevancy
    {
        public bool IsRelevant { get; set; }
    }
}