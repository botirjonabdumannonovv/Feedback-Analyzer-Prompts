using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddUserSecrets<Program>();
var configuration = configurationBuilder.Build();

var kernelBuilder = Kernel.CreateBuilder();

kernelBuilder.AddOpenAIChatCompletion(modelId: "gpt-3.5-turbo", apiKey: configuration["OpenAiApiSettings:ApiKey"]!);

var kernel = kernelBuilder.Build();

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

Console.Write("Enter your feedback about the product: ");
var userFeedback = Console.ReadLine();

var relevancePrompt = $@"
            Feedback Relevance Analysis:
                Objective: Determine whether the user feedback is relevant to specific products, services, or topics.
                
                Product Description:
                {productDescription}: A brief overview of the product or service being reviewed.
                User Feedback:
                {userFeedback}: Actual feedback provided by the user.
                Instructions:
                Define Criteria for Relevance: Establish what makes the feedback relevant to your product or service (e.g., mentions specific features, addresses service quality).
                Analyze Feedback: Assess each piece of feedback against the relevance criteria.
                Determine Relevance:
                If the feedback meets the established criteria, classify it as relevant (True).
                If it does not, classify it as irrelevant (False).
                Expected Output:
                Relevance Analysis: A boolean value indicating whether each piece of feedback is relevant ('True' for relevant, 'False' for irrelevant).
                Result:
                Feedback Relevance: A list showing whether each feedback item is considered relevant or not.
                Return Type: Return type must be boolean type (true/false). don't add extra information or detail. just return true or false.
        ";

// Invoke the Semantic Kernel for relevance analysis
var response = await kernel.InvokePromptAsync<string>(relevancePrompt);

// Create the FeedbackRelevancy model
var feedbackRelevancy = new FeedbackRelevancy { IsRelevant = Convert.ToBoolean(response) };

// Output the relevance result
Console.WriteLine($"Is the feedback relevant to the product or service? {feedbackRelevancy.IsRelevant}");

public class FeedbackRelevancy
{
    public bool IsRelevant { get; set; }
}
