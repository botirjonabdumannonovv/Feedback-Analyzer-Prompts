using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

// Main class
public class Program
{
    public static async Task Main(string[] args)
    {
        // Configuration setup for API key retrieval
        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddUserSecrets<Program>();
        var configuration = configurationBuilder.Build();

        // Semantic Kernel setup
        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOpenAIChatCompletion(modelId: "gpt-3.5-turbo", apiKey: configuration["OpenAiApiSettings:ApiKey"]!);
        var kernel = kernelBuilder.Build();

        // Sample product description
        const string productDescription = """
                                           Razor Viper Ultimate - wireless gaming mouse
                                                
                                                Sensitivity: 20,000DPI
                                                Tracking Speed : 650IPS
                                                Resolution accuracy : 99.6%
                                                
                                                74G LIGHTWEIGHT DESIGN
                                                Enjoy faster and smoother control with a lightweight wireless mouse designed for esports. Weighing just 74g, it achieves its weight without compromising on the build strength of its ambidextrous form factor.
                                                
                                                70 HOURS OF BATTERY LIFE
                                                Improved wireless power efficiency keeps it running at peak performance for up to 70 continuous hours—charge it just once a week to power 10 hours of daily gameplay.
                                                
                                                5 ON-BOARD MEMORY PROFILES
                                                Bring your settings anywhere and be match-ready in no time. Activate up to 5 profile configurations from its onboard memory or custom settings via cloud storage.
                                                
                                                8 PROGRAMMABLE BUTTONS
                                                Fully configurable via Razer Synapse 3, the 8 programmable buttons let you access macros and secondary functions so you can execute extended moves with ease. 
                                          """;
        // User feedback input
        Console.Write("Enter your feedback about the product: ");
        var userFeedback = Console.ReadLine();

        // Define the extraction prompt
        var extractionPrompt = $@"
            Actionable Questions Analysis from User Feedback:
                Objective: Examine user feedback to identify actionable questions that indicate areas for improvement, clarification, or further inquiry. These questions should be ones that can lead to tangible changes or responses from the product or service team.
                
                Instructions:
                Identify Actionable Questions:
                Review Feedback: Carefully analyze the user feedback provided.
                
                Extract Actionable Questions: Identify questions that imply a need for action, such as feature requests, bug reports, or clarifications about product usage.
                
                Analyze and Prioritize:
                Assess Urgency and Relevance: Evaluate the importance and immediacy of each actionable question based on the product's current objectives and user needs.
                
                Categorize by Theme: Organize the questions into thematic categories such as Usability, Functionality, Customer Support, etc., to streamline the response process.
                
                Document Recommendations:
                Propose specific actions or responses to each identified question, considering the potential impact on the user experience and product development.
                Context Provided:
                Product Description: {productDescription}
                User Feedback: {userFeedback}
                Expected Output:
                Actionable Questions: A list of identified questions from the user feedback that are directly actionable, accompanied by a brief analysis of their urgency, relevance, and proposed actions.
                Result:
                [Actionable Questions Analysis]: Present a structured analysis of each actionable question identified in the user feedback. This should include the question itself, its categorized theme, the assessed urgency, and recommended actions or responses.
        ";

        // Invoke the Semantic Kernel for feedback extraction
        var extractionResponse = await kernel.InvokePromptAsync<string>(extractionPrompt);

        // Assuming extractionResponse is formatted correctly, create and fill the UnifiedFeedback object
        var unifiedFeedback = new FeedbackExtraction { RelevantExtraction = extractionResponse };

        // Output the extracted feedback for verification
        Console.WriteLine($"{unifiedFeedback.RelevantExtraction}");
    }
    
    public class  FeedbackExtraction
    {
        public string? RelevantExtraction { get; set; }
    }
}