using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddUserSecrets<Program>();
var configuration = configurationBuilder.Build();

var kernelBuilder = Kernel.CreateBuilder();

kernelBuilder.AddOpenAIChatCompletion(modelId: "gpt-3.5-turbo", apiKey: configuration["OpenAiApiSettings:ApiKey"]!);

var kernel = kernelBuilder.Build();

string productDescription = "The Tesla Model Y: A symbol of innovation and sustainability in the automotive industry.";

Console.Write("Enter your feedback about the product: ");
var userFeedback = Console.ReadLine();

var relevancePrompt = $@"
            ## Detailed Instructions for Relevance Analysis:
        
        The primary objective is to meticulously examine the content of customer feedback to determine its applicability and relevance to our service offerings. You are expected to undertake this evaluation with precision, adhering to the conditions outlined below, and conclude with a verdict that categorically states whether the feedback is relevant ('True') or not ('False') based on our predefined criteria.
        
        ### Relevance Assessment Conditions:
        
        1. Mandatory Service Reference: The feedback must encompass at least one sentence that directly pertains to or mentions specific aspects of our service. This condition is non-negotiable and forms the cornerstone of the relevance determination.
        
        2. Inclusion of Unrelated Content: Should the feedback contain elements or narratives extraneous to our services, this will not automatically negate its relevance. The presence of unrelated content is permissible and will not disqualify the feedback, provided the Mandatory Service Reference condition (point 1) is fulfilled.
        
        ### Provided Data:
        
        - Service Description: {productDescription}
          This section provides a brief overview or detailed description of the service in question, serving as a reference point for evaluating the feedback's relevance.
        
        ### Input Requirement:
        
        - User Feedback: {userFeedback}
          This is the actual text of the feedback provided by the customer, which will be the subject of the relevance analysis.
        
        ### Expected Output Format:
        
        - [True/False]
          After thorough examination and analysis, indicate the relevance of the feedback to our service by marking 'True' if it meets the Mandatory Service Reference condition, or 'False' if it fails to do so.
        
        ### Additional Guidelines:
        
        - Ensure comprehensive analysis, taking into account the context and subtleties of the feedback.
        - Maintain objectivity and impartiality during the assessment process.
        - Document the reasoning behind your relevance determination to support transparency and understanding.
        - Don't give any extra information about user feedback. You have to return only boolean type (True or False).
        
        ## Result only [True/False]:
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
